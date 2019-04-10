using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDust : Spell
{
    protected override void OnCollisionEnter(Collision collision) {
        if (!collisions) { return; }
        DestroyComponents();
        if (explosion) { Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3f); }
        TerrainManager.instance.AntiCircle(Mathf.RoundToInt(transform.position.x)
            , Mathf.RoundToInt(transform.position.y)
            , Mathf.RoundToInt(transform.position.z),
            (int)damageRadius, explosionDampen);
        Player[] players = FindObjectsOfType<Player>();

        for (int i = 0; i < players.Length; i++) {
            Player player = players[i];
            Vector3 playerDirection = players[i].transform.position - collision.GetContact(0).point;
            if (playerDirection.magnitude < damageRadius) {
                if (playersHit.Contains(players[i])) { return; }
                player.Damage(contactDamage);
                player.GetComponent<Rigidbody>().Sleep();
                playerDirection.Normalize();
                player.rigidbody.AddForce(playerDirection.x * knockBackForce, (playerDirection.y + 1) * knockBackForce,
                    playerDirection.z * knockBackForce);
                playersHit.Add(players[i]);
            }
        }
    }
}
