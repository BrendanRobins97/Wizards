// File: TerrainManager.cs
// Author: Brendan Robinson
// Date Created: 02/22/2019
// Date Last Modified: 02/28/2019

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class TerrainManager : MonoBehaviour {

    #region Constants

    private const float voxelSize = 1f;

    public static TerrainManager instance;
    public GameObject treePrefab;
    public GameObject[] rockPrefabs;
    [Range(0,100)]public int chanceToSpawn = 60;
    [Range(0.0f, 10.0f)] public float slopeValue = 8f;
    #endregion

    #region Fields

    [SerializeField] private float          octaves     = 5;
    [SerializeField] private float          smoothness  = 50f;
    [SerializeField] private float          scale       = 25f;
    [SerializeField] private float          persistence = 0.5f;
    [SerializeField] private float          lacunarity  = 0.5f;
    [SerializeField] private AnimationCurve heightMap;
    [SerializeField] private Chunk          chunkPrefab;

    private bool firstPass = true;
    private int            width     = 128;
    private int            length    = 128;
    private int            numChunks = 8;
    private int            chunkSize;
    private int count = 0;
    private float          counter = 0;
    private Vector3[,]     meshPoints;
    private MeshFilter     filter;
    private MeshCollider   collider;
    private Chunk[,]       chunks;
    private HashSet<Chunk> chunksToUpdate = new HashSet<Chunk>();
    private List<GameObject> spawnedItems = new List<GameObject>();
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
        //Start of a check to see if:
        //1). the item was spawned above a hole
        //2). if it was spawned on too steep of a slope.
        //only checks a fixed number of times after everything has spawned in.
        
        if (firstPass && (count <= 255))
        {
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                Debug.Log(spawnedItems[i]);
               
                RaycastHit hit, hitUp;
                Vector3 downRayEnd = new Vector3(spawnedItems[i].transform.position.x,
                    spawnedItems[i].transform.position.y - 10, spawnedItems[i].transform.position.z);
                Ray downRay = new Ray(spawnedItems[i].transform.position, downRayEnd);
                
                Ray upRay = new Ray(spawnedItems[i].transform.position, Vector3.up);
                if (Physics.Raycast(upRay, out hitUp, 20f))
                {
                    Debug.Log("Deactivated due to underground" + spawnedItems[i].name);
                    Debug.DrawLine(spawnedItems[i].transform.position, Vector3.up, Color.yellow, 100f);
                    spawnedItems[i].SetActive(false);
                }
                if (Physics.Raycast(downRay, out hit,10f))
                {
                    float distanceToCollision = hit.distance;
                    Debug.Log(distanceToCollision);
                    if (distanceToCollision > 5)
                    {
                        Debug.Log("Deactivated due to hole" + spawnedItems[i].name);
                        spawnedItems[i].SetActive(false);
                    }
                }
                RaycastHit hitNorth, hitEast, hitSouth, hitWest;
                 
                Ray downRayNorth = new Ray(new Vector3(spawnedItems[i].transform.position.x+1.5f, spawnedItems[i].transform.position.y, spawnedItems[i].transform.position.z), -Vector3.up);
                Ray downRayEast = new Ray(new Vector3(spawnedItems[i].transform.position.x, spawnedItems[i].transform.position.y, spawnedItems[i].transform.position.z+1.5f), -Vector3.up);
                Ray downRaySouth = new Ray(new Vector3(spawnedItems[i].transform.position.x-1.5f, spawnedItems[i].transform.position.y, spawnedItems[i].transform.position.z), -Vector3.up);
                Ray downRayWest = new Ray(new Vector3(spawnedItems[i].transform.position.x, spawnedItems[i].transform.position.y, spawnedItems[i].transform.position.z-1.5f), -Vector3.up);
                if (Physics.Raycast(downRayNorth, out hitNorth))
                {
                    float distanceToCollision = Mathf.Abs(hitNorth.distance);
                    if (distanceToCollision > slopeValue)
                    {
                        Debug.Log("Deactivated due to slope " + spawnedItems[i].name + distanceToCollision);
                        spawnedItems[i].SetActive(false);
                    }
                }
                if (Physics.Raycast(downRayEast, out hitEast))
                {
                    float distanceToCollision = Mathf.Abs(hitEast.distance);
                    if (distanceToCollision > slopeValue )
                    {
                        Debug.Log("Deactivated due to slope " + spawnedItems[i].name + distanceToCollision);
                        spawnedItems[i].SetActive(false);
                    }
                }
                if (Physics.Raycast(downRaySouth, out hitSouth))
                {
                    float distanceToCollision = Mathf.Abs(hitSouth.distance);
                    if (distanceToCollision > slopeValue)
                    {
                        Debug.Log("Deactivated due to slope " + spawnedItems[i].name + distanceToCollision);
                        spawnedItems[i].SetActive(false);
                    }
                }
                if (Physics.Raycast(downRayWest, out hitWest))
                {
                    float distanceToCollision = Mathf.Abs(hitWest.distance);
                    if (distanceToCollision > slopeValue)
                    {
                        Debug.Log("Deactivated due to slope " + spawnedItems[i].name + distanceToCollision);
                        spawnedItems[i].SetActive(false);
                    }
                }

                count++;
                //firstPass = false;
            }
        }//end of spawn check.
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
                /*float random1 = Random.Range(0, 500);
                if (random1 < chanceToSpawn)
                {
                    Vector3 spawnPoint = new Vector3(x * voxelSize, yCoordinate, z * voxelSize);
                    float random2 = Random.Range(0, 100);
                    if (random2 < 60)
                    {
                        Instantiate(treePrefab, spawnPoint, treePrefab.transform.rotation);
                        treePrefab.SetActive(true);
                        spawnedItems.Add(treePrefab);
                    }
                    else
                    {
                        int randomIndex = Random.Range(0, rockPrefabs.Length);
                        Instantiate(rockPrefabs[randomIndex], spawnPoint,
                            rockPrefabs[randomIndex].transform.rotation);
                        spawnedItems.Add(rockPrefabs[randomIndex]);
                    }
                }*/
            }
        }
        for (int i = 0; i < numChunks; i++) {
            for (int j = 0; j < numChunks; j++) {
                chunks[i, j] = Instantiate(chunkPrefab);               
                chunks[i, j].position = new Int2(i * chunkSize, j * chunkSize);
                chunks[i, j].width = chunkSize;
                chunks[i, j].length = chunkSize;
                chunks[i, j].UpdateChunk(ref meshPoints);
                int random1 = Random.Range(0, 100);
                if (random1 <= chanceToSpawn && (i > 0 || i != numChunks) && (j > 0 || j != numChunks))
                {
                    Vector3 spawnPoint = new Vector3(chunks[i, j].position.x, chunks[i, j].transform.localPosition.y + 15f, chunks[i, j].position.y);
                    RaycastHit hit;
                    Ray downRay = new Ray(spawnPoint, -Vector3.up);
                    float newY = 0;
                    if (Physics.Raycast(downRay, out hit))
                    {
                        float distanceToCollision = hit.distance;
                        Debug.Log(distanceToCollision);
                        newY = chunks[i, j].transform.localPosition.y + 15f - distanceToCollision;
                        spawnPoint =
                            hit.point; //new Vector3(chunks[i, j].position.x, newY-.8f, chunks[i, j].position.y);
                    }
                    int random2 = Random.Range(0, 100);
               
                    if (random2 < 40)
                    {
                        treePrefab.transform.position = spawnPoint;
                        Instantiate(treePrefab, treePrefab.transform.position, treePrefab.transform.rotation);
                        treePrefab.SetActive(true);
                        spawnedItems.Add(treePrefab);
                    }
                    else
                    {
                        int randomIndex = Random.Range(0, rockPrefabs.Length);
                        Instantiate(rockPrefabs[randomIndex], spawnPoint,
                            rockPrefabs[randomIndex].transform.rotation);
                        spawnedItems.Add(rockPrefabs[randomIndex]);
                    } 
                }
            }
        }

        firstPass = true;//allows for spawn check in the Update function to run
        
    }

    #endregion

}