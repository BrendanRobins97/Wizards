using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstuff : MonoBehaviour {

    public GameObject hit;
    public GameObject Instance;

	void OnTriggerEnter(Collider col)
    {
        Instance = Instantiate(hit);
        Instance.transform.position = transform.position;
        Instance.transform.rotation = transform.rotation;
        Instance.SetActive(true);
        Destroy(gameObject);
        Destroy(Instance, 2f);
    }
}
