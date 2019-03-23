﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRainSpell : Spell
{
    
    public override void ThrowSpell(Vector3 direction, float charge)
    {
        DeathRainSpellCamera drc = FindObjectOfType<DeathRainSpellCamera>();
        CameraBehavior cam = FindObjectOfType<CameraBehavior>();
        if(cam.enabled == true)
        { drc.spellCamera.enabled = false;}
        direction = new Vector3(drc.lightPos.x, drc.lightPos.y+5,drc.lightPos.z);
        transform.position = direction;
        this.transform.Rotate(-90,0,0,0);
        Disable(duration);
    }

   /* void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Player>().Damage(this.contactDamage);
        }
    }*/
}
