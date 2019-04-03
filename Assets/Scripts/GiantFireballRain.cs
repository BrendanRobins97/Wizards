using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantFireballRain : MonoBehaviour
{
    //[SerializeField] private Spell explostiveShot;
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
            Destroy(col.gameObject);
        }

        if (col.tag == "Chunk")
        {
            col.gameObject.SetActive(false);
            //Instantiate(explostiveShot, col.transform.position, transform.rotation);
        }

        
    }
}
