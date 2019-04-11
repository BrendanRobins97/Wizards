using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRainParticleBehavior : MonoBehaviour {

    public float radius = 8f;
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
        Vector3 randNormal = new Vector3((2 * Random.value - 1) * 0.1f, -0.5f,
            (2 * Random.value - 1) *0.1f);
        Vector2 randCircle = Random.insideUnitCircle * radius;
        Vector3 randPosition = transform.position + new Vector3(randCircle.x, 0, randCircle.y);
        Spell smallExplosion = Instantiate(smallExplosiveShot, randPosition, Quaternion.identity);
        smallExplosion.speed = Random.Range(2, 24);
        smallExplosion.ThrowSpell(randNormal, 1);
        // Disable collisions to start so it doesn't instantly collide with who it hit.
        smallExplosion.DisableCollisions(0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
