using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBehavior : MonoBehaviour
{
    [SerializeField] private float maxGroundAngle = 20;
    private float angle;
    private AssetBehavior[] assetList;
    private RaycastHit hitInfo;

    private AssetSpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<AssetSpawner>();
        assetList = FindObjectsOfType<AssetBehavior>();
        maxGroundAngle = spawner.maxGroundAngle;
        Physics.Raycast(this.transform.position, -Vector3.up, out hitInfo);
        angle = Vector3.Angle(hitInfo.normal, this.transform.forward);
        Debug.Log("Angle " + angle + " hit info normal " + hitInfo.normal);
        if (angle >= maxGroundAngle)
        {
            Destroy(this.gameObject);
        }

        for (int i = 0; i < assetList.Length; i++)
        {
            if (Mathf.Abs(Vector3.Distance(assetList[i].transform.position, this.transform.position)) < 8)
            {
                this.gameObject.transform.position = new Vector3(Random.Range(spawner.min,spawner.max), 24, Random.Range(spawner.min, spawner.max));
            }
            Random randomSequence = new Random();
            Random.InitState(randomSequence.GetHashCode());
            for (int j = 0; j < assetList.Length; j++)
            {
                if (Mathf.Abs(Vector3.Distance(assetList[i].transform.position, this.transform.position)) < 8)
                {
                    this.gameObject.transform.position = new Vector3(Random.Range(spawner.min, spawner.max), 24, Random.Range(spawner.min, spawner.max));
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "FireSpell" || col.tag == "Spell")
        {
            Debug.Log("unfreezing");
            this.gameObject.GetComponent<Rigidbody>().constraints &= RigidbodyConstraints.FreezePositionX;
            this.gameObject.GetComponent<Rigidbody>().constraints &= RigidbodyConstraints.FreezePositionZ;
            this.gameObject.GetComponent<Rigidbody>().constraints &= RigidbodyConstraints.FreezePositionY;
        }
    }
}
