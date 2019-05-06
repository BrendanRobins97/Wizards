// File: CameraController.cs
// Contributors: Brendan Robinson
// Date Created: 05/06/2019
// Date Last Modified: 05/06/2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Constants

    public static CameraController instance;

    #endregion

    #region Fields

    private List<Transform> cameras = new List<Transform>();
    private List<Vector3> originalPositions = new List<Vector3>();

    #endregion

    #region Methods

    private void Awake() { instance = this; }

    public void RegisterCamera(Transform camera) {
        cameras.Add(camera);
        originalPositions.Add(camera.transform.position);
        for (int i = 0; i < cameras.Count; i++)
        {
            Debug.Log(cameras[i].name);
        }
    }

    public void ScreenShakeAll(float shakeAmount = 0.35f, int numFrames = 20) {
        for (int i = 0; i < cameras.Count; i++) { StartCoroutine(ScreenShakeCoroutine(i, shakeAmount, numFrames)); }
    }

    public void ScreenShake(Transform camera, float shakeAmount = 0.35f, int numFrames = 20) {
        StartCoroutine(ScreenShakeCoroutine(cameras.IndexOf(camera), shakeAmount, numFrames));
    }

    private IEnumerator ScreenShakeCoroutine(int camIndex, float shakeAmount, int numFrames) {
        for (int i = 0; i < numFrames; i++) {
            cameras[camIndex].transform.position = originalPositions[camIndex] + new Vector3(
                                                       Random.Range(-shakeAmount, shakeAmount) *
                                                       (1 - (float) i / numFrames),
                                                       Random.Range(-shakeAmount, shakeAmount) *
                                                       (1 - (float) i / numFrames),
                                                       0);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
        cameras[camIndex].transform.position = originalPositions[camIndex];
    }

    #endregion

}