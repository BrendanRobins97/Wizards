// File: WaterBall.cs
// Contributors: Brendan Robinson
// Date Created: 05/08/2019
// Date Last Modified: 05/08/2019

using System.Collections;
using UnityEngine;

public class WaterBall : Spell {

    #region Fields

    public Spell smallExplosiveShot;
    public int numSmallExplosions = 4;

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
        if (rigidbody) { transform.right = -rigidbody.velocity; }
    }

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        soundPlay = GameObject.Find("soundManager");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playFireBallEnd();
        // Instantiate smaller explosions on contact;

        StartCoroutine(WaterSpout());
    }

    private IEnumerator WaterSpout() {
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < numSmallExplosions; i++) {
            // Jitter start direction based on normal
            float angle = i * 360f / numSmallExplosions;

            
            Vector3 randNormal = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 1,
                Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Lerp(0.75f, 1.5f, (float) i / numSmallExplosions));
            //randNormal.Normalize();
            Spell smallExplosion = Instantiate(smallExplosiveShot, transform.position, Quaternion.identity);
            smallExplosion.ThrowSpell(randNormal, 1);
            // Disable collisions to start so it doesn't instantly collide with who it hit.
            smallExplosion.DisableCollisions(0.1f);

            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion

}