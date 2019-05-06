using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThornBall : Spell {

    #region Fields

    public Spell smallExplosiveShot;
    public int numSmallExplosions = 8;

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

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        soundPlay = GameObject.Find("soundManager");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playFireBallEnd();
        // Instantiate smaller explosions on contact;
        
        for (int i = 0; i < numSmallExplosions; i++) {
            // Jitter start direction based on normal
            float angle = i * 360f / numSmallExplosions;

            Vector3 randNormal = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0,
                Mathf.Sin(angle * Mathf.Deg2Rad));
            randNormal.Normalize();
            Spell smallExplosion = Instantiate(smallExplosiveShot, transform.position, Quaternion.identity);
            smallExplosion.ThrowSpell(randNormal, 1);
            // Disable collisions to start so it doesn't instantly collide with who it hit.
            smallExplosion.DisableCollisions(0.15f);
        }

    }

    #endregion

}
