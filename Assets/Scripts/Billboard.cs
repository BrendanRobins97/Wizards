// File: Billboard.cs
// Contributors: Brendan Robinson
// Date Created: 05/12/2019
// Date Last Modified: 05/12/2019

using UnityEngine;

public class Billboard : MonoBehaviour {

    #region Fields

    private Camera[] cameras;

    #endregion

    #region Methods

    // Update is called once per frame
    private void Update() {
        cameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in cameras) {
            if (cam.enabled) { transform.LookAt(cam.transform); }
        }
    }

    #endregion

}