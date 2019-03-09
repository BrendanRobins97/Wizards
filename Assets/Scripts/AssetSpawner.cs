﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetSpawner : MonoBehaviour
{

    public int numItemsToSpawn;
    [SerializeField] private float min = 10.0f;
    [SerializeField] private float max = 124.0f;
    [SerializeField] private GameObject [] assetPrefab;
    [SerializeField] private float maxGroundAngle = 20;
    private float angle;
    private float bestDistance = 5;
    //private float bestCandidateX = new float();
    //private float bestCandidateZ = new float();
    private float defaultY = 24.0f;
    private int numCandidates = 10;
    private RaycastHit hitInfo;
    private List<Vector3> usedPoints = new List<Vector3>();
    private List<GameObject> assetList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(10);
        for (int i = 0; i < numItemsToSpawn; i++)
        {
            Random.InitState(i * i);

            Vector3 position = new Vector3(Random.Range(min, max), defaultY, Random.Range(min, max));
            for (int k = 0; k < usedPoints.Count; k++)
            {
                if (Mathf.Abs(usedPoints[k].x - position.x) < 6)
                {
                    position.x += 10;
                    assetList[k].transform.position = new Vector3(assetList[k].transform.position.x - 5,
                        assetList[k].transform.position.y, assetList[k].transform.position.z);
                    if (position.x >= max)
                    {
                        position.x = max - 2;
                    }
                }

                if (Mathf.Abs(usedPoints[k].z - position.z) < 6)
                {
                    position.z += 10;
                    assetList[k].transform.position = new Vector3(assetList[k].transform.position.x,
                        assetList[k].transform.position.y, assetList[k].transform.position.z - 5);
                    if (position.z >= max)
                    {
                        position.z = max - 2;
                    }
                }
            }

            int randomIndex = Random.Range(0, assetPrefab.Length);
            usedPoints.Add(position);
            Instantiate(assetPrefab[randomIndex], position, assetPrefab[randomIndex
        ].transform.rotation);
            assetList.Add(assetPrefab[randomIndex]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    Vector3 Spawn()
    {
        float bestCandidateX = new float();
        float bestCandidateZ = new float();
        for (int i = 0; i < numCandidates; i++)
        {  

            float x = Random.Range(min, max);
            float z = Random.Range(min, max);
            //FVector2D temp(x, y);
            float minDistance = 100000.0f;
            for (int j = 0; j < usedPoints.Count; j++)
            {
                Debug.Log("usedPoint.Count " + usedPoints.Count);
                //(uint32)usedPoints.TArray::Num() // blueNoise[j].myArray
                float dx = Mathf.Abs((usedPoints[j].x) - (x));
                float dz = Mathf.Abs((usedPoints[j].z) - (z));
                Debug.Log("dx " + dx + " dz " + dz);
                //float dist = FVector2D::DistSquared(temp, blueNoise.myArray[j]);
                float dist = ((dx * dx) + (dz * dz));//x - blueNoise.myArray[j].X;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    
                }
                Debug.Log("MinDistance " + minDistance);
            }
            if (minDistance > bestDistance)
            {
                bestDistance = minDistance;
                bestCandidateX = x;
                bestCandidateZ = z;
                Debug.Log("X " + x + " Z " + z);
                Debug.Log("bestX " + bestCandidateX + " bestZ " + bestCandidateZ);
            }
        }
        Debug.Log("Spawn asset " + new Vector3(bestCandidateX, defaultY, bestCandidateZ));
        //Instantiate(assetPrefab, new Vector3(bestCandidateX, defaultY, bestCandidateZ), assetPrefab.transform.rotation);
        //assetPrefab.SetActive(true);
        usedPoints.Add(new Vector3(bestCandidateX, defaultY, bestCandidateZ));
        numItemsToSpawn--;
        return new Vector3(bestCandidateX, defaultY, bestCandidateZ);
    }
}
