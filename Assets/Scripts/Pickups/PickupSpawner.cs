using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] pickupList;
    private AssetSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<AssetSpawner>();
        SpawnPickups();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPickups()
    {
        for (int i = 0; i < pickupList.Length; i++)
        {
            Vector3 position = new Vector3(Random.Range(spawner.min+20, spawner.max-20), 34, Random.Range(spawner.min, spawner.max));
            pickupList[i].transform.position = position;
            pickupList[i].transform.rotation = pickupList[i].transform.rotation;
        }
    }
}
