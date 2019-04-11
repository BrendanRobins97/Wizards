using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Spell
{
    public override void ThrowSpell(Vector3 direction, float charge) {
        if (!affectedByCharge) { charge = 1; }
        transform.forward = direction;
        Disable(duration);
    }

    protected override void OnCollisionEnter(Collision collision) {
        
    }

    protected void OnTriggerEnter(Collider other) {
        if (!collisions) { return; }
        if (explosion) { Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3f); }
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.gameObject.GetComponent<Player>();
            Vector3 playerDirection = player.transform.position - transform.position;

            player.Damage(contactDamage);
            player.GetComponent<Rigidbody>().Sleep();
            playerDirection.Normalize();
            player.rigidbody.AddForce(playerDirection.x * knockBackForce, (playerDirection.y / 4f + 1f) * knockBackForce,
                playerDirection.z * knockBackForce);
        }
    }
}
