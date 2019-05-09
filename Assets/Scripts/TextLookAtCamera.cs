using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLookAtCamera : MonoBehaviour
{
    private CameraBehavior cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.spellCamera.transform);
    }
}
