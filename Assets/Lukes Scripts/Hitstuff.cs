using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstuff : MonoBehaviour {

    public GameObject hit;
    public GameObject Instance;

	void OnCollisionEnter(Collision collision)
    {
        Instance = Instantiate(hit);
        Instance.transform.position = transform.position;
        Instance.transform.rotation = transform.rotation;
        Instance.SetActive(true);
        Destroy(Instance, 3f);
    }
}
