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

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);

        // Instantiate smaller explosions on contact;
        Vector3 normal = collision.GetContact(0).normal;
        for (int i = 0; i < numSmallExplosions; i++) {
            // Jitter start direction based on normal
            Vector3 randNormal = new Vector3(normal.x + (2 * Random.value - 1), normal.y,
                normal.z + (2 * Random.value - 1));
            randNormal.Normalize();
            Instantiate(smallExplosiveShot, transform.position, Quaternion.identity).ThrowSpell(randNormal, 1);
        }
    }

    #endregion

}