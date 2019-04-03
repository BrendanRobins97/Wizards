// File: TerrainManager2.cs
// Author: Brendan Robinson
// Date Created: 03/05/2019
// Date Last Modified: 03/05/2019

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainManager2 : MonoBehaviour {

    #region Constants

    private const float voxelSize = 1f;

    public static TerrainManager2 instance;

    #endregion

    #region Fields

    [SerializeField] private float octaves         = 5;
    [SerializeField] private float smoothness      = 50f;
    [SerializeField] private float scale           = 25f;
    [SerializeField] private float persistence     = 0.5f;
    [SerializeField] private float lacunarity      = 0.5f;
    [SerializeField] private float groundThickness = 4f;

    [SerializeField] private AnimationCurve  heightMap;
    [SerializeField] private Chunk2          chunkPrefab;
    private                  Cell[,,]        grid;
    [HideInInspector] public int             width     = 128;
    [HideInInspector] public int             length    = 128;
    private                  int             height    = 64;
    private                  int             chunkSize = 16;
    private                  Vector3Int      numChunks;
    private                  Chunk2[,,]      chunks;
    private                  HashSet<Chunk2> chunksToUpdate = new HashSet<Chunk2>();
    private                  float           seed;

    #endregion

    #region Methods

    private void Awake() {
        if (instance != null) { Destroy(this); }
        else { instance = this; }
    }

    private void Start() {
        seed = Random.Range(-1000f, 1000f);
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
                }
            }
        }

        numChunks = new Vector3Int(width / chunkSize, height / chunkSize, length / chunkSize);
        chunks = new Chunk2[numChunks.x, numChunks.y, numChunks.z];
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

    private void Update() {
        // Update all chunks that need to be updated
        foreach (Chunk2 chunk in chunksToUpdate) { chunk.UpdateChunk(ref grid); }
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
                    float density =  Mathf.Max(0, -(i * i + j * j / heightDampen + k * k - radius * radius) / (float)(radius * radius / 2f));
                    UpdatePosition(new Vector3(x + i, y + j, z + k), density + SampleDensity(x + i, y + j, z + k));
                }
            }
        }
    }

    public void AntiCircle(int x, int y, int z, int radius, float heightDampen = 1) {

        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                for (int k = -radius; k <= radius; k++) {
                    float density = -Mathf.Max(0, -(i * i + j * j / heightDampen + k * k - radius * radius) / (float)(radius * radius / 2f));
                    UpdatePosition(new Vector3(x + i, y + j, z + k), density + SampleDensity(x + i, y + j, z + k));
                }
            }
        }
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