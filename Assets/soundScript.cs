using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class soundScript : MonoBehaviour
{
    public AudioSource audioData;
    public AudioClip mainTheme;
    public AudioClip fireballStart;
    public AudioClip fireballEnd;
    public AudioClip satanOn;
    public AudioClip rightWrong;
    public AudioClip hairFoot;
    public AudioClip dunnoHowWorks;
    public AudioClip badMan;
    public AudioClip basicBitch;
    public AudioClip zap;
    public AudioClip Oof;
    public AudioClip myOof;
    public AudioClip YIIPE;
    // Start is called before the first frame update        
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void playFireBallStart()
    {
        audioData.clip = fireballStart;
        audioData.Play();
        return;
    }
    public void playFireBallEnd()
    {
        audioData.clip = fireballEnd;
        audioData.Play();
        return;
    }
    public void playZap()
    {
        audioData.clip = zap;
        audioData.Play();
        return;
    }
    public void playOof()
    {
        int myint = Random.Range(0, 3);
        if (myint == 1)
        {
            audioData.clip = Oof;
            audioData.Play();
            return;
        }
        if (myint == 2)
        {
            audioData.clip = myOof;
            audioData.Play();
            return;
        }
        if (myint == 3)
        {
            audioData.clip = YIIPE;
            audioData.Play();
            return;
        }
    }
    public void playPlayerStart()
    {
        int myint = Random.Range(0, 9);
        if (myint == 1 || myint == 7){
            audioData.clip = satanOn;
            audioData.Play();
            return;
        }
        if (myint == 2 || myint == 8)
        {
            audioData.clip = rightWrong;
            audioData.Play();
            return;
        }
        if (myint == 3)
        {
            audioData.clip = hairFoot;
            audioData.Play();
            return;
        }
        if (myint == 4 || myint == 9)
        {
            audioData.clip = dunnoHowWorks;
            audioData.Play();
            return;
        }
        if (myint == 5)
        {
            audioData.clip = badMan;
            audioData.Play();
            return;
        }
        if (myint == 6)
        {
            audioData.clip = basicBitch;
            audioData.Play();
            return;
        }
    }
}
