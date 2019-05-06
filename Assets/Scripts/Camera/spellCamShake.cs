using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellCamShake : MonoBehaviour
{
    public static spellCamShake instance;

    public GameObject spellCamContainer;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void Awake() { instance = this; }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ScreenShake(Transform camera, float shakeAmount = 0.5f, int numFrames = 25)
    {
        StartCoroutine(ScreenShakeCoroutine(0, shakeAmount, numFrames));
    }
    private IEnumerator ScreenShakeCoroutine(int camIndex, float shakeAmount, int numFrames)
    {
        for (int i = 0; i < numFrames; i++)
        {
            spellCamContainer.transform.position = spellCamContainer.transform.position + new Vector3(
                                                       Random.Range(-shakeAmount, shakeAmount) *
                                                       (1 - (float)i / numFrames),
                                                       Random.Range(-shakeAmount, shakeAmount) *
                                                       (1 - (float)i / numFrames),
                                                       0);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
    }

}
