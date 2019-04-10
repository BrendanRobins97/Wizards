using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstuff : MonoBehaviour {

    public GameObject hit;
    public GameObject Instance;
    public GameObject soundPlay;

    void OnCollisionEnter(Collision collision)
    {
        Instance = Instantiate(hit);
        Instance.transform.position = transform.position;
        Instance.transform.rotation = transform.rotation;
        Instance.SetActive(true);
        soundPlay = GameObject.Find("soundManager");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playFireBallEnd();
        Destroy(Instance, 3f);
    }
}
