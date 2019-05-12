// File: BoulderRain.cs
// Contributors: Brendan Robinson
// Date Created: 04/26/2019
// Date Last Modified: 05/12/2019

using System.Collections.Generic;
using UnityEngine;

public class BoulderRain : Spell {

    #region Fields

    public float sizeIncreaseRate = 1f;
    public LayerMask groundMask;

    private float disableTime = .2f;
    private float size;
    private Vector3 prevPosition;
    private Collider collider;
    private List<Player> playersHit = new List<Player>();
    private float startSize = 1;
    private float timeBetweenTerrainDestroy = 0.15f;
    private float destroyCounter;
    private bool settled;
    private float timeSinceAlive;

    #endregion

    #region Methods

    protected override void Start() {
        base.Start();
        startSize = transform.localScale.x;
        size = startSize; // Get the start size
        prevPosition = Vector3.one;
    }

    private void FixedUpdate() {
        Vector3 dist = prevPosition - transform.position;
        prevPosition = transform.position;
        timeSinceAlive += Time.deltaTime;
        destroyCounter -= Time.fixedDeltaTime;
        if (Physics.CheckSphere(transform.position, size + 0.25f, groundMask)) {
            size += Time.deltaTime * sizeIncreaseRate * dist.magnitude;
            if (!settled && destroyCounter <= 0) {
                TerrainManager.instance.Circle(Mathf.RoundToInt(transform.position.x)
                    , Mathf.RoundToInt(transform.position.y)
                    , Mathf.RoundToInt(transform.position.z),
                    (int) (damageRadius * (1 + (size - startSize) / 6f)), explosionDampen);
                destroyCounter = timeBetweenTerrainDestroy;
            }
        }

        transform.localScale = new Vector3(size, size, size);
        if (dist.magnitude <= 0.11f && timeSinceAlive >= 2f) { settled = true; }
    }

    public override void ThrowSpell(Vector3 direction, float charge) {
        charge = 0;
        DeathRainSpellCamera drc = FindObjectOfType<DeathRainSpellCamera>();
        CameraBehavior cam = FindObjectOfType<CameraBehavior>();
        cam.enabled = true;
        if (cam.enabled) {
            disableTime -= Time.deltaTime;
            if (disableTime <= 0) { drc.spellCamera.enabled = false; }
        }

        drc.spellHitPointIndicator.enabled = false;
        direction = new Vector3(drc.lightPos.x, TerrainManager.instance.height - 12f, drc.lightPos.z);
        transform.position = direction;
        transform.Rotate(-90, 0, 0, 0);
        Disable(duration);
    }

    protected override void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            if (!playersHit.Contains(player)) {
                // Increase damage the greater size it is
                player.Damage((int) (contactDamage * (1 + (size - startSize) / 6f)));
                playersHit.Add(player);
            }
        }
    }

    #endregion

}