// File: TerrainManager.cs
// Author: Brendan Robinson
// Date Created: 02/22/2019
// Date Last Modified: 02/28/2019

using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    #region Constants

    private const float voxelSize = 1f;

    public static TerrainManager instance;

    #endregion

    #region Fields

    [SerializeField] private float          octaves     = 5;
    [SerializeField] private float          smoothness  = 50f;
    [SerializeField] private float          scale       = 25f;
    [SerializeField] private float          persistence = 0.5f;
    [SerializeField] private float          lacunarity  = 0.5f;
    [SerializeField] private AnimationCurve heightMap;
    [SerializeField] private Chunk          chunkPrefab;

    private int            width     = 128;
    private int            length    = 128;
    private int            numChunks = 8;
    private int            chunkSize;
    private float          counter = 0;
    private Vector3[,]     meshPoints;
    private MeshFilter     filter;
    private MeshCollider   collider;
    private Chunk[,]       chunks;
    private HashSet<Chunk> chunksToUpdate = new HashSet<Chunk>();

    #endregion

    #region Methods

    private void Awake() {
        if (instance != null) { Destroy(this); }
        else { instance = this; }
    }

    private void Start() {
        filter = GetComponent<MeshFilter>();
        collider = GetComponent<MeshCollider>();
        chunks = new Chunk[numChunks, numChunks];
        chunkSize = width / numChunks;

        GenerateTerrain();

        Vector3 meshPoint = meshPoints[67, 36];
        Circle(meshPoint.x, meshPoint.y, meshPoint.z, 12, 0.5f);
    }

    private void Update() {
        // Update all chunks that need to be updated
        foreach (Chunk chunk in chunksToUpdate) { chunk.UpdateChunk(ref meshPoints); }
        chunksToUpdate.Clear();
    }

    public void UpdatePosition(float x, float z, Vector3 newValue) {
        int xPoint = (int) x;
        int zPoint = (int) z;
        meshPoints[xPoint, zPoint] = newValue;
        UpdateChunk(xPoint, zPoint);
        if (xPoint % chunkSize == 0) { UpdateChunk(xPoint - 1, zPoint); }
        if (xPoint % chunkSize == chunkSize - 1) { UpdateChunk(xPoint + 1, zPoint); }
        if (zPoint % chunkSize == 0) { UpdateChunk(xPoint, zPoint - 1); }
        if (zPoint % chunkSize == chunkSize - 1) { UpdateChunk(xPoint, zPoint + 1); }
    }

    public void UpdateChunk(int x, int y) {
        x /= chunkSize;
        y /= chunkSize;
        if (x < 0 || x >= numChunks || y < 0 || y >= numChunks) { return; }
        chunksToUpdate.Add(chunks[x, y]);
    }

    public void Circle(float x, float y, float z, float radius, float heightDampen = 1) {
        Vector3 meshPoint = meshPoints[(int) x, (int) z];
        x = meshPoint.x;
        y = meshPoint.y;
        z = meshPoint.z;

        for (float i = -radius; i < radius; i++) {
            for (float j = -radius; j < radius; j++) {
                if (i * i + j * j < radius * radius) {
                    float minY = Mathf.Min(y - Mathf.Sqrt(-i * i - j * j + radius * radius) * heightDampen,
                        meshPoints[(int) (i + x), (int) (j + z)].y);
                    UpdatePosition((int) (i + x), (int) (j + z),
                        new Vector3(meshPoints[(int) (i + x), (int) (j + z)].x, minY
                            , meshPoints[(int) (i + x), (int) (j + z)].z));
                }
            }
        }
    }

    private void GenerateTerrain() {
        meshPoints = new Vector3[width + 1, length + 1];
        for (int x = 0; x <= width; x++) {
            for (int z = 0; z <= length; z++) {
                float yCoordinate = Utilities.PerlinNoise(x, z, smoothness, scale, octaves, persistence, lacunarity);
                yCoordinate *= heightMap.Evaluate(yCoordinate / scale);
                meshPoints[x, z] = new Vector3(x * voxelSize, yCoordinate, z * voxelSize);
            }
        }
        for (int i = 0; i < numChunks; i++) {
            for (int j = 0; j < numChunks; j++) {
                chunks[i, j] = Instantiate(chunkPrefab);
                chunks[i, j].position = new Int2(i * chunkSize, j * chunkSize);
                chunks[i, j].width = chunkSize;
                chunks[i, j].length = chunkSize;

                chunks[i, j].UpdateChunk(ref meshPoints);
            }
        }
    }

    #endregion

}