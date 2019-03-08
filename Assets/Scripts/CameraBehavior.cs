﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private GameManager gm;
    private Player player;
    [SerializeField] private GameObject spell, fireSpell;
    private float xpos, zpos;
    private float tempY = 7;
    public Camera spellCamera;

    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
        spellCamera.enabled = false;
        gm = FindObjectOfType<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        xpos = player.transform.position.x;
        zpos = player.transform.position.z;
        spell = GameObject.FindGameObjectWithTag("Spell");
        fireSpell = GameObject.FindGameObjectWithTag("FireSpell");
        if ((spell == null && fireSpell == null) || gm.currentTurnTimeLeft > gm.timeAfterSpellCast)
        {
            spellCamera.enabled = false;
            Destroy(spell,gm.timeAfterSpellCast);
            Debug.Log("Spell Cam = disabled.");
        }
        if (spell != null && gm.currentTurnTimeLeft <= gm.timeAfterSpellCast)
        {
            ChangeToSpellCamera();
        }
        if ( (spell == null && fireSpell == null) || gm.currentTurnTimeLeft > gm.timeAfterSpellCast)
        {
            spellCamera.enabled = false;
            Destroy(fireSpell, gm.timeAfterSpellCast);
            tempY = 7;
            Debug.Log("Fireball Cam = disabled.");
        }
        if (fireSpell != null && gm.currentTurnTimeLeft <= gm.timeAfterSpellCast)
        {
            ChangeToFireballCamera();
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
}