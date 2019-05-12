using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDust : Spell
{

    public virtual void ThrowSpell(Vector3 direction, float charge) {
        // Disable collisions for a millisecond after casting so
        // it doesn't instantly collide with player throwing spell
        DisableCollisions(0.2f);
        if (!affectedByCharge) { charge = 1; }
        charge = Mathf.Max(charge, 0.33f); // Min charge is 1/3rd
        GetComponent<Rigidbody>().velocity = direction * charge * speed;
        transform.forward = direction;
        Disable(duration);
    }

    protected override void OnCollisionEnter(Collision collision) {
        if (!collisions) { return; }
        DestroyComponents();
        if (explosion) { Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3f); }
        
        Invoke("SpawnDust", 0.2f);
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

    private void SpawnDust() {
        TerrainManager.instance.AntiCircle(Mathf.RoundToInt(transform.position.x)
            , Mathf.RoundToInt(transform.position.y)
            , Mathf.RoundToInt(transform.position.z),
            (int)damageRadius, explosionDampen);
    }
}
