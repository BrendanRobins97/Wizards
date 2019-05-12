using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRainSpell : Spell
{
    private float disableTime = .2f;
    public override void ThrowSpell(Vector3 direction, float charge)
    {
        charge = 0; 
        DeathRainSpellCamera drc = FindObjectOfType<DeathRainSpellCamera>();
        CameraBehavior cam = FindObjectOfType<CameraBehavior>();
        cam.enabled = true;
        if (cam.enabled == true)
        {
            disableTime -= Time.deltaTime;
            if (disableTime <= 0)
            {
                drc.spellCamera.enabled = false;
            }
            
        }

        drc.spellHitPointIndicator.enabled = false;
        direction = new Vector3(drc.lightPos.x, drc.lightPos.y+5,drc.lightPos.z);
        transform.position = direction;
        this.transform.Rotate(-90,0,0,0);
        Disable(timeAfterSpellCast);
    }

   /*void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Player>().Damage(this.contactDamage);
        }
    }*/
}
