using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderShot : Spell
{
    protected virtual void OnCollisionEnter(Collision collision) {
        if (!collisions) { return; }
        if (collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            if (playersHit.Contains(player)) { return; }
            Vector3 playerDirection = player.transform.position - collision.GetContact(0).point;

            player.Damage(contactDamage);
            player.GetComponent<Rigidbody>().Sleep();
            playerDirection.Normalize();
            player.rigidbody.AddForce(playerDirection.x * knockBackForce, (playerDirection.y + 1) * knockBackForce,
                playerDirection.z * knockBackForce);
            playersHit.Add(player);
        }
    }
}
