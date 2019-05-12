// File: ExplosiveShot.cs
// Author: Brendan Robinson
// Date Created: 03/01/2019
// Date Last Modified: 03/01/2019

using UnityEngine;

public class HammerShot : Spell {

    #region Fields

    public Spell smallExplosiveShot;
    public int   numSmallExplosions = 4;

    #endregion

    #region Methods
    protected override void Start() {
        base.Start();
        soundPlay = GameObject.Find("soundManager");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playFireBallStart();
        //soundPlay.GetComponent<soundScript>().playFireBall1();
    }
    protected void Update() {
        if (rigidbody) {
            transform.right = -rigidbody.velocity;
        }
    }

    public override void ThrowSpell(Vector3 direction, float charge) {
        // Disable collisions for a millisecond after casting so
        // it doesn't instantly collide with player throwing spell
        DisableCollisions(0.13f);
        if (!affectedByCharge) { charge = 1; }
        charge = Mathf.Max(charge, 0.33f); // Min charge is 1/3rd
        GetComponent<Rigidbody>().velocity = direction * charge * speed;
        transform.forward = direction;
        Disable(duration);
    }

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        soundPlay = GameObject.Find("soundManager");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playFireBallEnd();
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