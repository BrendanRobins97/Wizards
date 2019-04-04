// File: ExplosiveShot.cs
// Author: Brendan Robinson
// Date Created: 03/01/2019
// Date Last Modified: 03/01/2019

using UnityEngine;

public class ExplosiveShot : Spell {

    #region Fields

    public Spell smallExplosiveShot;
    public int   numSmallExplosions = 4;

    #endregion

    #region Methods

    protected void Update() {
        if (rigidbody) {
            transform.right = -rigidbody.velocity;
        }
    }

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);

        // Instantiate smaller explosions on contact;
        
        for (int i = 0; i < numSmallExplosions; i++) {
            // Jitter start direction based on normal
            Vector3 randNormal = new Vector3((2 * Random.value - 1), 1,
                (2 * Random.value - 1));
            randNormal.Normalize();
            Spell smallExplosion = Instantiate(smallExplosiveShot, transform.position, Quaternion.identity);
            smallExplosion.ThrowSpell(randNormal, 1);
            // Disable collisions to start so it doesn't instantly collide with who it hit.
            smallExplosion.DisableCollisions(0.25f);
        }
        
    }

    #endregion

}