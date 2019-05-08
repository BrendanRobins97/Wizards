using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHit : Spell
{
    public GameObject hitPoint;
    
    public override void ThrowSpell(Vector3 direction, float charge)
    {
        if (!affectedByCharge) { charge = 1; }
        transform.forward = direction;
        hitPoint.transform.parent = GameManager.instance.CurrentPlayer.hitPoint;
        hitPoint.SetActive(false);
        Disable(duration);
    }

    void Update()
    {
        
    }
    public void EnableCollider()
    {
        hitPoint.SetActive(true);
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (!collisions) { return; }
        if (explosion) { Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3f); }
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Player>() != GameManager.instance.CurrentPlayer)
        {
            Player player = other.gameObject.GetComponent<Player>();   
            Vector3 playerDirection = player.transform.position - transform.position;

            //soundPlay = GameObject.Find("soundManager");
            //soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
            //sound.playIce();
            player.Damage(contactDamage);
            player.GetComponent<Rigidbody>().Sleep();
            //playerDirection.Normalize();
            player.rigidbody.AddForce(playerDirection.x * knockBackForce, (playerDirection.y / 4f + 1f) * knockBackForce,
                playerDirection.z * knockBackForce);
        }
    }
}
