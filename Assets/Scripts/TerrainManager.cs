// File: TerrainManager.cs
// Contributors: Brendan Robinson
// Date Created: 04/03/2019
// Date Last Modified: 04/10/2019

using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    #region Constants

    private const float voxelSize = 1f;

    public static TerrainManager instance;

    #endregion

    #region Fields

    [Header("Terrain Settings")] 

    public int width = 128;
    public int length = 128;
    public int height = 64;
    [Space]
    [SerializeField] private float octaves = 5;
    [SerializeField] private float          smoothness      = 50f;
    [SerializeField] private float          scale           = 25f;
    [SerializeField] private float          persistence     = 0.5f;
    [SerializeField] private float          lacunarity      = 0.5f;
    [SerializeField] private float          groundThickness = 4f;
    [SerializeField] private AnimationCurve heightMap;
    [SerializeField] private bool spawnAssets = true;
    [SerializeField] private byte numAssets = 12;



    [Header("Environment Prefabs")] [SerializeField]
    private GameObject boulder1;
    [SerializeField] private GameObject boulder2;
    [SerializeField] private GameObject boulder3;
    [SerializeField] private GameObject boulder4;
    [SerializeField] private GameObject boulder5;
    [SerializeField] private GameObject tree1;
    [SerializeField] private GameObject tree2;
    [SerializeField] private GameObject tree3;

    [Space] [SerializeField] private Chunk chunkPrefab;

    private Cell[,,]       grid;
    
    private int            chunkSize = 16;
    private Vector3Int     numChunks;
    private Chunk[,,]      chunks;
    private HashSet<Chunk> chunksToUpdate = new HashSet<Chunk>();
    public float          seed;

    private RandomArray<GameObject> boulders;
    private RandomArray<GameObject> trees;

    #endregion

    #region Methods

    private void Awake() {
        if (instance != null) { Destroy(this); }
        else { instance = this; }

        // Random array of assets to spawn
        // All assets have an equal spawn chance
        boulders = new RandomArray<GameObject>(
            new[] {boulder1, boulder2, boulder3, boulder4, boulder5},
            new[] {1f, 1f, 1f, 1f, 1f});
        trees = new RandomArray<GameObject>(
            new[] {tree1, tree2, tree3},
            new[] {1f, 1f, 1f});

        //seed = Random.Range(-100f, 100f);
        grid = new Cell[width + 1, height + 1, length + 1];

        for (int x = 0; x <= width; x++) {
            for (int z = 0; z <= length; z++) {
                float yCoordinate =
                    Utilities.PerlinNoise(x + seed, z, smoothness, scale, octaves, persistence, lacunarity);
                yCoordinate *= heightMap.Evaluate(yCoordinate / scale);
                yCoordinate += groundThickness;
                for (int y = 0; y <= height; y++) {
                    grid[x, y, z].Point = new Vector3(x * voxelSize, y * voxelSize, z * voxelSize);
                    grid[x, y, z].Density = y - yCoordinate;
                    // This makes a sphere...
                    float dist = Utilities.DistToSphereSurface(x - width / 2f, z - length / 2f, y - height / 2f,
                        width / 2f);
                    if (dist > 0) { grid[x, y, z].Density = dist; }
                }
            }
        }

        numChunks = new Vector3Int(width / chunkSize, height / chunkSize, length / chunkSize);
        chunks = new Chunk[numChunks.x, numChunks.y, numChunks.z];
        for (int i = 0; i < numChunks.x; i++) {
            for (int j = 0; j < numChunks.y; j++) {
                for (int k = 0; k < numChunks.z; k++) {
                    chunks[i, j, k] = Instantiate(chunkPrefab);
                    chunks[i, j, k].position = new Vector3Int(i * chunkSize, j * chunkSize, k * chunkSize);
                    chunks[i, j, k].size = chunkSize;
                    chunks[i, j, k].UpdateChunk(ref grid);
                }
            }
        }
    }

    private void Start() {
        if (spawnAssets) {
            for (int i = 0; i < numAssets;) {
                const float bounds = 16f; // Bigger value means points will be closer to center
                float randPointX = Random.Range(bounds, width - bounds);
                float randPointZ = Random.Range(bounds, length - bounds);
                RaycastHit hit = PeakPoint(randPointX, randPointZ);

                if (hit.collider.tag == "Chunk" && hit.distance < height) {
                    Vector3 spawnPoint = hit.point;

                    if (hit.normal.y >= 0.92) {
                        Instantiate(trees.RandomItem(), spawnPoint + new Vector3(0, -1f, 0), Quaternion.identity);
                        i++;
                    } else if (hit.normal.y >= 0.8) {
                        Instantiate(boulders.RandomItem(), spawnPoint + new Vector3(0, -1f, 0), Quaternion.identity);
                        i++;
                    }
                }
            }
        }
        
    }

    private void Update() {
        // Update all chunks that need to be updated
        foreach (Chunk chunk in chunksToUpdate) { chunk.UpdateChunk(ref grid); }
        chunksToUpdate.Clear();
    }

    public void UpdatePosition(Vector3 position, float density) {
        Vector3Int point = Utilities.Vector3IntFromVector3(position);

        // Do nothing if the point is not in bounds
        if (point.x < 0 || point.x >= width
                        || point.y < 0 || point.y >= height
                        || point.z < 0 || point.z >= length) { return; }
        // Do nothing if its the same value as before
        if (grid[point.x, point.y, point.z].Density == density) { return; }

        // Update the density and update chunk/bordering chunks
        grid[point.x, point.y, point.z].Density = density;
        UpdateChunk(point.x, point.y, point.z);
        if (point.x % chunkSize == 0) { UpdateChunk(point.x - 1, point.y, point.z); }
        if (point.x % chunkSize == chunkSize - 1) { UpdateChunk(point.x + 1, point.y, point.z); }
        if (point.y % chunkSize == 0) { UpdateChunk(point.x, point.y - 1, point.z); }
        if (point.y % chunkSize == chunkSize - 1) { UpdateChunk(point.x, point.y + 1, point.z); }
        if (point.z % chunkSize == 0) { UpdateChunk(point.x, point.y, point.z - 1); }
        if (point.z % chunkSize == chunkSize - 1) { UpdateChunk(point.x, point.y, point.z + 1); }
    }

    public void UpdateChunk(int x, int y, int z) {
        x /= chunkSize;
        y /= chunkSize;
        z /= chunkSize;
        if (x < 0 || x >= numChunks.x || y < 0 || y >= numChunks.y || z < 0 || z >= numChunks.x) { return; }
        chunksToUpdate.Add(chunks[x, y, z]);
    }

    public void Circle(int x, int y, int z, int radius, float heightDampen = 1) {
        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                for (int k = -radius; k <= radius; k++) {
                    float density = Mathf.Max(0,
                        -(i * i + j * j / heightDampen + k * k - radius * radius) / (radius * radius / 2f));
                    UpdatePosition(new Vector3(x + i, y + j, z + k), density + SampleDensity(x + i, y + j, z + k));
                }
            }
        }
    }

    public void AntiCircle(int x, int y, int z, int radius, float heightDampen = 1) {
        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                for (int k = -radius; k <= radius; k++) {
                    float density = -Mathf.Max(0,
                        -(i * i + j * j / heightDampen + k * k - radius * radius) / (radius * radius / 2f));
                    UpdatePosition(new Vector3(x + i, y + j, z + k), density + SampleDensity(x + i, y + j, z + k));
                }
            }
        }
    }

    // Returns a raycast to the highest y value at a specified x, z
    public RaycastHit PeakPoint(float x, float z) {
        Ray ray = new Ray(new Vector3(x, height, z), Vector3.down);
        Physics.Raycast(ray, out RaycastHit rayHit);
        return rayHit;
    }

    private float SampleDensity(int x, int y, int z) {
        if (x < 0 || x >= width
                  || y < 0 || y >= height
                  || z < 0 || z >= length) { return 0; }
        return grid[x, y, z].Density;
    }

    private float SampleDensity(float x, float y, float z) {
        return SampleDensity(Mathf.RoundToInt(x), Mathf.RoundToInt(y), Mathf.RoundToInt(z));
    }

    #endregion

}