using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonMelee : Spell
{
    public GameObject hitPoint1;//, hitPoint2, hitPoint3, hitPoint4;

    public override void ThrowSpell(Vector3 direction, float charge)
    {
        if (!affectedByCharge) { charge = 1; }
        transform.forward = direction;
        hitPoint1.transform.parent = GameManager.instance.CurrentPlayer.demonHit1;
        hitPoint1.GetComponent<BoxCollider>().enabled = false;
        //hitPoint1.SetActive(false);
        //hitPoint2.transform.parent = GameManager.instance.CurrentPlayer.demonHit2;
        //hitPoint2.SetActive(false);
        //hitPoint3.transform.parent = GameManager.instance.CurrentPlayer.demonHit3;
        //hitPoint3.SetActive(false);
        //hitPoint4.transform.parent = GameManager.instance.CurrentPlayer.demonHit4;
        //hitPoint4.SetActive(false);
        Disable(duration);
    }

    void Update()
    {
        
    }
    public void EnableCollider()
    {
        hitPoint1.GetComponent<BoxCollider>().enabled = true;
        //hitPoint2.SetActive(true);
        //hitPoint3.SetActive(true);
        //hitPoint4.SetActive(true);
        Destroy(this.gameObject,4f);
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
            playerDirection.Normalize();
            player.rigidbody.AddForce(playerDirection.x * knockBackForce, (playerDirection.y / 4f + 1f) * knockBackForce,
                playerDirection.z * knockBackForce);
        }
    }
}
