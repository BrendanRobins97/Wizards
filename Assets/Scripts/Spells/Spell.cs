// File: Spell.cs
// Author: Brendan Robinson
// Date Created: 02/19/2019
// Date Last Modified: 03/01/2019

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell", order = 0)]
public class Spell : MonoBehaviour {

    #region Fields

    public    float        speed;
    public    int          contactDamage;
    public    bool         affectedByCharge;
    public    float        duration     = 10f;
    public    float        damageRadius = 5f;
    public float explosionDampen = .75f;

    protected List<Player> playersHit   = new List<Player>();

    #endregion

    #region Methods

    // Start is called before the first frame update
    protected void Start() { Destroy(gameObject, 10); }

    // Update is called once per frame
    protected void Update() { }

    public void ThrowSpell(Vector3 direction, float charge) {
        if (!affectedByCharge) { charge = 1; }
        GetComponent<Rigidbody>().velocity = direction * charge * speed;
        transform.forward = direction;
        Disable(duration);
    }

    public void Disable(float time) { Invoke("DestroyComponents", time); }

    protected virtual void OnCollisionEnter(Collision collision) {
        DestroyComponents();
        Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
        TerrainManager.instance.Circle(transform.position.x, transform.position.y, transform.position.z,
            damageRadius, explosionDampen);
        Player[] players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++) {
            if ((players[i].transform.position - collision.GetContact(0).point).magnitude < damageRadius) {
                if (playersHit.Contains(players[i])) { return; }
                players[i].Damage(contactDamage);
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
    }

    #endregion

}