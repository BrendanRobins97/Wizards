using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRainParticleBehavior : MonoBehaviour
{

    public ParticleSystem ps;
    public Spell smallExplosiveShot;
    public Spell spell;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void OnParticleCollision(GameObject collision)
    {
        Debug.Log("Colliding with " + collision.name);
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().Damage(spell.contactDamage);
            Debug.Log("Damaging " + collision.name);
        }

        // Jitter start direction based on normal
        Vector3 randNormal = new Vector3((2 * Random.value - 1), 1,
            (2 * Random.value - 1));
        randNormal.Normalize();
        Spell smallExplosion = Instantiate(smallExplosiveShot, transform.position, Quaternion.identity);
        Spell smallExplosion2 = Instantiate(smallExplosiveShot, collision.transform.position, Quaternion.identity);
        
        smallExplosion.ThrowSpell(randNormal, 1);
        smallExplosion2.ThrowSpell(randNormal, 1);
        // Disable collisions to start so it doesn't instantly collide with who it hit.
        smallExplosion.DisableCollisions(0.25f);
        smallExplosion2.DisableCollisions(0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
