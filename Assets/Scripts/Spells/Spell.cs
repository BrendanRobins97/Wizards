﻿// File: Spell.cs
// Contributors: Brendan Robinson
// Date Created: 05/06/2019
// Date Last Modified: 05/12/2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell", order = 0)]
public class Spell : MonoBehaviour {

    #region Fields

    public float speed;
    public int contactDamage;
    public bool affectedByCharge;
    public float duration = 10f;
    public float damageRadius = 5f;
    public float explosionDampen = .75f;
    public float knockBackForce;
    public GameObject explosion;
    public Sprite spellImage;
    public Color spellImageColor;
    public float timeAfterSpellCast = 3f;
    public string description = "";

    protected List<Player> playersHit = new List<Player>();
    protected bool collisions = true;
    protected Rigidbody rigidbody;
    protected GameObject soundPlay;

    #endregion

    #region Methods

    protected virtual void Start() {
        Destroy(gameObject, duration);
        rigidbody = GetComponent<Rigidbody>();
        if (gameObject.name == "LightningPrefab(Clone)") { //play a lightning sound effect if this is lightning
            soundPlay = GameObject.Find("soundManager");
            soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
            sound.playZap();
        }
    }

    public virtual void ThrowSpell(Vector3 direction, float charge) {
        // Disable collisions for a millisecond after casting so
        // it doesn't instantly collide with player throwing spell
        DisableCollisions(0.04f);
        if (!affectedByCharge) { charge = 1; }
        charge = Mathf.Max(charge, 0.33f); // Min charge is 1/3rd
        GetComponent<Rigidbody>().velocity = direction * charge * speed;
        transform.forward = direction;
        Disable(duration);
    }

    public void Disable(float time) { Invoke("DestroyComponents", time); }

    public void DisableCollisions(float time) { StartCoroutine("DisableCollisionsRoutine", time); }

    protected virtual void OnCollisionEnter(Collision collision) {
        if (!collisions) { return; }
        DestroyComponents();
        if (gameObject.name == "SmallExplosiveShot(Clone)") {//HammerDustShots(Clone)
            soundPlay = GameObject.Find("soundManager");
            soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
            sound.playFireBallEnd();
        }
        else if (gameObject.name == "HammerThrow(Clone)")
        {
            soundPlay = GameObject.Find("soundManager");
            soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
            sound.playHammer();
        }
        else if (gameObject.name == "Lightning(Clone)")
        {
            soundPlay = GameObject.Find("soundManager");
            soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
            sound.playZap();
        }
        else if (gameObject.name == "FireStream(Clone)")
        {
            soundPlay = GameObject.Find("soundManager");
            soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
            sound.playZap();
        }

        if (explosion) { Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3f); }
        TerrainManager.instance.Circle(Mathf.RoundToInt(transform.position.x)
            , Mathf.RoundToInt(transform.position.y)
            , Mathf.RoundToInt(transform.position.z),
            (int) damageRadius, explosionDampen);
        CameraController.instance.ScreenShakeAll(0.6f, 60);
        Player[] players = FindObjectsOfType<Player>();

        for (int i = 0; i < players.Length; i++) {
            Player player = players[i];
            Vector3 playerDirection = players[i].transform.position - collision.GetContact(0).point;
            if (playerDirection.magnitude < damageRadius) {
                if (playersHit.Contains(players[i])) { return; }
                player.Damage(contactDamage);
                player.GetComponent<Rigidbody>().Sleep();
                playerDirection.Normalize();
                player.rigidbody.AddForce(playerDirection.x * knockBackForce,
                    (playerDirection.y / 4f + 1f) * knockBackForce,
                    playerDirection.z * knockBackForce);
                playersHit.Add(players[i]);
            }
        }
    }

    protected void DestroyComponents() {
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();
        MeshRenderer rend = GetComponent<MeshRenderer>();
        if (rb) { Destroy(rb); }
        if (col) { Destroy(col); }
        if (rend) { Destroy(rend); }

        foreach (Transform child in transform) { Destroy(child.gameObject); }
    }

    protected IEnumerator DisableCollisionsRoutine(float time) {
        collisions = false;
        yield return new WaitForSeconds(time);
        collisions = true;
    }

    #endregion

}