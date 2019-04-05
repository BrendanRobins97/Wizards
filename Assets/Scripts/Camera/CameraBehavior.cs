using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject spell, fireSpell, deathRainSpell, iceSpell;
    //[SerializeField] private Animator anim;
    private float xpos, zpos;
    private float tempY = 7;
    private bool positionSet = false;
    private DeathRainSpellCamera drc;
    public Camera spellCamera;
    private Canvas canvas;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        drc = FindObjectOfType<DeathRainSpellCamera>();
        //anim = FindObjectOfType<Animator>();
        spellCamera.enabled = false;
        canvas = FindObjectOfType<Canvas>();
        //anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        player = GameManager.instance.GetComponent<GameManager>().CurrentPlayer;
        if (spellCamera.enabled == true)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }
        xpos = player.transform.position.x;
        zpos = player.transform.position.z;
        spell = GameObject.FindGameObjectWithTag("Spell");
        fireSpell = GameObject.FindGameObjectWithTag("FireSpell");
        deathRainSpell = GameObject.FindGameObjectWithTag("DeathRainSpell");
        iceSpell = GameObject.FindGameObjectWithTag("IceSpell");
        /*if (drc.camera.enabled)
        {
            ChangeToRainSpellCamera();
        }*/
        if (player.casting)
        {
            Debug.Log("Casting");
            spellCamera.enabled = true;
            //spellCamera.transform.position = new Vector3(xpos+5,5,zpos+5);
            //spellCamera.transform.LookAt(player.transform);
        }
        if ((spell == null && fireSpell == null && deathRainSpell == null ) || GameManager.instance.currentTurnTimeLeft > GameManager.instance.timeAfterSpellCast)
        {
            spellCamera.enabled = false;
            Destroy(spell, GameManager.instance.timeAfterSpellCast);
            positionSet = false;
            Debug.Log("Spell Cam = disabled.");
        }
        if (spell != null && GameManager.instance.currentTurnTimeLeft <= GameManager.instance.timeAfterSpellCast)
        {
            ChangeToSpellCamera();
        }
        if ( (spell == null && fireSpell == null && deathRainSpell == null && iceSpell == null) || GameManager.instance.currentTurnTimeLeft > GameManager.instance.timeAfterSpellCast)
        {
            spellCamera.enabled = false;
            Destroy(fireSpell, GameManager.instance.timeAfterSpellCast);
            Destroy(iceSpell, GameManager.instance.timeAfterSpellCast+1);
            tempY = 7;
            positionSet = false;
            Debug.Log("Fireball Cam = disabled.");
        }
        if (fireSpell != null && GameManager.instance.currentTurnTimeLeft <= GameManager.instance.timeAfterSpellCast)
        {
            ChangeToFireballCamera();
        }
        if (deathRainSpell != null && GameManager.instance.currentTurnTimeLeft <= GameManager.instance.timeAfterSpellCast)
        {
            ChangeToDeathRainCamera();
        }
        if (iceSpell != null && GameManager.instance.currentTurnTimeLeft <= GameManager.instance.timeAfterSpellCast)
        {
            ChangeToIceSpellCamera();
        }
    }

    public void ChangeToFireballCamera()
    {
        tempY -= (Time.deltaTime * 1.5f);
        spellCamera.enabled = true;
        Debug.Log("Fireball Cam = enabled.");
        float x, z, finalX, finalZ;
        x = (xpos + (fireSpell.transform.position.x+2))/2.0f;
        z = (zpos + (fireSpell.transform.position.z-2)) / 2.0f;
        finalX = (x + fireSpell.transform.position.x+2) / 2.0f;
        finalZ = (z + fireSpell.transform.position.z-2) / 2.0f;
        spellCamera.transform.position = new Vector3(finalX, fireSpell.transform.position.y + tempY ,finalZ);
            //spellCamera.transform.Rotate(0,speed*Time.deltaTime,0);
            if (fireSpell.isStatic)
            {
                spellCamera.transform.position = Vector3.MoveTowards(spellCamera.transform.position, fireSpell.transform.position,speed*Time.deltaTime);
            }

            spellCamera.transform.LookAt(fireSpell.transform);
    }

    public void ChangeToSpellCamera()
    {
        Debug.Log("Spell Cam = enabled.");
        spellCamera.enabled = true;
        spellCamera.transform.SetPositionAndRotation(
            new Vector3(spell.transform.position.x + 1, spell.transform.position.y + 2,
                spell.transform.position.z - 6), Quaternion.identity);
        //fireBallCamera.transform.position = new Vector3(spell.transform.position.x + 1, spell.transform.position.y + 2,
        //  spell.transform.position.z - 6);
    }

    public void ChangeToDeathRainCamera()
    {
        Debug.Log("RainSpellCam");
        spellCamera.enabled = true;
        float x, z, finalX, finalZ;
        x = (xpos + (deathRainSpell.transform.position.x + 2)) / 2.0f;
        z = (zpos + (deathRainSpell.transform.position.z - 2)) / 2.0f;
        //finalX = (x + deathRainSpell.transform.position.x + 2) / 2.0f;
        //finalZ = (z + deathRainSpell.transform.position.z - 2) / 2.0f;
        if (positionSet == false)
        {
            //spellCamera.transform.position = new Vector3(x, deathRainSpell.transform.position.y -2, z);
            spellCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, player.transform.position.z);
            spellCamera.transform.LookAt(new Vector3(deathRainSpell.transform.position.x,deathRainSpell.transform.position.y-2, deathRainSpell.transform.position.z));
            positionSet = true;           
        }
        spellCamera.transform.LookAt(new Vector3(deathRainSpell.transform.position.x, deathRainSpell.transform.position.y - 2, deathRainSpell.transform.position.z));
        spellCamera.transform.position = Vector3.MoveTowards(spellCamera.transform.position,new Vector3(player.transform.localPosition.x, spellCamera.transform.position.y+2, player.transform.localPosition.z-3),  speed*Time.deltaTime);

    }

    public void ChangeToIceSpellCamera()
    {
        spellCamera.enabled = true;

        if (positionSet == false)
        {
            //spellCamera.transform.forward = player.transform.position - player.playerCamera.transform.position;
            spellCamera.transform.position = new Vector3(iceSpell.transform.position.x + 6, iceSpell.transform.position.y + 2,
                iceSpell.transform.position.z + 1);
            spellCamera.transform.LookAt(iceSpell.transform);
            positionSet = true;
        }
        spellCamera.transform.position = Vector3.MoveTowards(spellCamera.transform.position, iceSpell.transform.position, speed * Time.deltaTime);
    }
}
