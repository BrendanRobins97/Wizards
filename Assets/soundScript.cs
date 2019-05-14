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
    public AudioClip ice;
    public AudioClip Oof;
    public AudioClip myOof;
    public AudioClip YIIPE;
    public AudioClip BS_freaks;
    public AudioClip BS_grind;
    public AudioClip BS_mallet;
    public AudioClip BS_notWiz;
    public AudioClip BS_oof;
    public AudioClip FG_croak;
    public AudioClip FG_hop;
    public AudioClip FG_swamp;
    public AudioClip FG_oof;
    public AudioClip PL_amp;
    public AudioClip PL_booster;
    public AudioClip PL_hypo;
    public AudioClip PL_contagious;
    public AudioClip PL_oof;
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
    public void playIce()
    {
        audioData.clip = ice;
        audioData.Play();
        return;
    }
    public void playOof(string name)//Chado Icon Demon Icon Frogizard Icon Bauta Icon
    {
        if (name == "Chado Icon")
            audioData.clip = BS_oof;
        else if (name == "Bauta Icon")
            audioData.clip = PL_oof;
        else if (name == "Frogizard Icon")
            audioData.clip = FG_oof;
        //int myint = Random.Range(0, 3);
        //if (myint == 1)
        //{
        //    audioData.clip = Oof;
        //    audioData.Play();
        //    return;
        //}
        //if (myint == 2)
        //{
        //    audioData.clip = myOof;
        //    audioData.Play();
        //    return;
        //}
        //if (myint == 3)
        //{
        //    audioData.clip = YIIPE;
        //    audioData.Play();
        //    return;
        //}
    }
    public void playPlayerStart(string name)
    {
        
        int myint = Random.Range(0, 7);
        if (myint == 1 || myint == 5){
            if(name == "Chado Icon")
                audioData.clip = BS_freaks;
            else if (name == "Bauta Icon")
                audioData.clip = PL_amp;
            else if (name == "Frogizard Icon")
                audioData.clip = FG_croak;
            audioData.Play();
            return;
        }
        if (myint == 2 || myint == 6)
        {
            if (name == "Chado Icon")
                audioData.clip = BS_grind;
            else if (name == "Bauta Icon")
                audioData.clip = PL_booster;
            else if (name == "Frogizard Icon")
                audioData.clip = FG_hop;
            audioData.Play();
            return;
        }
        if (myint == 3)
        {
            //print(name);
            if (name == "Chado Icon")
                audioData.clip = BS_notWiz;
            else if (name == "Bauta Icon")
                audioData.clip = PL_hypo;
            else if (name == "Frogizard Icon")
                audioData.clip = FG_swamp;
            audioData.Play();
            return;
        }
        if (myint == 4 || myint == 7)
        {
            if (name == "Chado Icon")
                audioData.clip = BS_mallet;
            else if (name == "Bauta Icon")
                audioData.clip = PL_contagious;
            else if (name == "Frogizard Icon")
                audioData.clip = FG_swamp;
            audioData.Play();
            return;
        }
        if (myint == 8)
        {
            audioData.clip = badMan;
            audioData.Play();
            return;
        }
        if (myint == 9)
        {
            audioData.clip = basicBitch;
            audioData.Play();
            return;
        }
        if(myint >= 10){
            return;//don't play a sound a 4th of the time
        }
    }
}
