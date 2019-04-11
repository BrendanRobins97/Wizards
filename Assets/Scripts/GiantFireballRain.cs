using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GiantFireballRain : MonoBehaviour
{
    [SerializeField] private Spell explosiveShot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject,4f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag =="Player")
        {
            //Destroy(col.gameObject);
            col.GetComponent<Player>().Damage(100);
        }

        if (col.tag == "Chunk" || col.tag == "Tree")
        {
            //col.gameObject.SetActive(false);
            Instantiate(explosiveShot, transform.position, transform.rotation);
            Destroy(this.gameObject,.2f);
        }
        
    }
}
