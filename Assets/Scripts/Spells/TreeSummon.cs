using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSummon : Spell
{
    private Vector3 finalPos;

    private float untagTime;
    // Start is called before the first frame update
    void Start()
    {
        untagTime = duration;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * .3f);
        untagTime -= Time.deltaTime;
        if (untagTime <= 0)
        {
            gameObject.tag = "Untagged";
        }
    }
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
        direction = new Vector3(drc.lightPos.x, drc.lightPos.y -17f, drc.lightPos.z);
        transform.position = direction;
        finalPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        //this.transform.Rotate(-90, 0, 0, 0);
        Disable(duration * 100000f);
    }
}
