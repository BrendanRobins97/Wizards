using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreenSpell : Spell
{
    private float disableTime = .2f;
    private float damageTime = .1f;
    private float untagTime = 0f;
    private Player spellOwner;
    private int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        spellOwner = GameManager.instance.CurrentPlayer;
        untagTime = timeAfterSpellCast - .5f;
        playerNumber = GameManager.instance.playerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        damageTime -= Time.deltaTime;
        untagTime -= Time.deltaTime;
        if (untagTime < 0)
        {
            gameObject.tag = "Untagged";
        }

        if ((spellOwner == GameManager.instance.CurrentPlayer || spellOwner == null) && untagTime < -1)
        {
            Disable(0);
        }
        
    }

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
        direction = new Vector3(drc.lightPos.x, drc.lightPos.y - 6, drc.lightPos.z);
        transform.position = direction;
        //this.transform.Rotate(-90, 0, 0, 0);
       
        //Disable(timeAfterSpellCast);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && damageTime > 0)
        {
            if(col.gameObject.GetComponent<Player>() != GameManager.instance.CurrentPlayer)
                col.GetComponent<Player>().Damage(contactDamage);
        }
    }
}
