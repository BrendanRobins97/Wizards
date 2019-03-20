using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRainSpell : Spell
{
    /*public DeathRainSpellCamera spellCamera;
    [SerializeField] private Player player,currentPlayer;
    private Vector3 startPos;
    private float maxX, minX, maxZ, minZ;
    public float camSpeed;
    public GameObject prefab;
    [SerializeField] private GameObject spellHitPointIndicator;

    private GameObject currentSpell;
    */
    public override void ThrowSpell(Vector3 direction, float charge)
    {
        //currentPlayer.enabled = true;
        //Instantiate(prefab, direction, Quaternion.identity);
        //this.enabled = false;
        //spellCamera.spellCamera.enabled = true;
        //spellHitPointIndicator.SetActive(false);
        DeathRainSpellCamera drc = FindObjectOfType<DeathRainSpellCamera>();
        CameraBehavior cam = FindObjectOfType<CameraBehavior>();
        if(cam.enabled == true)
        { drc.spellCamera.enabled = false;}
        direction = new Vector3(drc.lightPos.x, drc.lightPos.y+5,drc.lightPos.z);
        transform.position = direction;
        Disable(duration);
    }

    
}
