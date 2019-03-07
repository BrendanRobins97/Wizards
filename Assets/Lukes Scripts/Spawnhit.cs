using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnhit : MonoBehaviour {

    public GameObject hit;
    public GameObject Instance;

	void OnTriggerEnter(Collider col)
    {
        Instance = Instantiate(hit);
        Instance.transform.position = col.gameObject.transform.position;
        Instance.transform.rotation = transform.rotation;
        Instance.SetActive(true);
        Destroy(Instance, 2f);
    }
}
