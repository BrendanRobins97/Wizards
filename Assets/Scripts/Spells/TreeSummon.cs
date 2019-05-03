using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSummon : Spell {

    public float summonDuration;

    public float startPositionHeightOffset = 10f;

    private Vector3 startPos;

    private Vector3 finalPos;

    public Collider[] collidersToEnable;

    private float untagTime;
    private float timeSinceAlive = 0;
    // Start is called before the first frame update
    void Start()
    {
        untagTime = duration;
    }

    // Update is called once per frame
    void Update() {
        timeSinceAlive += Time.deltaTime;
        
        transform.position = Vector3.Lerp(startPos, finalPos, Mathf.Clamp01(timeSinceAlive / summonDuration));
        if (timeSinceAlive >= summonDuration + 0.1f) {
            foreach (Collider col in collidersToEnable) { col.enabled = true; } // Emable hitbox
            GetComponent<SphereCollider>().enabled = false; // Disable trigger so it doesn't deal any more damage;
        }
        untagTime -= Time.deltaTime;
        if (untagTime <= 0)
        {
            gameObject.tag = "Untagged";
        }
    }
    private float disableTime = .2f;

    public override void ThrowSpell(Vector3 direction, float charge)
    {
        DeathRainSpellCamera drc = FindObjectOfType<DeathRainSpellCamera>();
        CameraBehavior cam = FindObjectOfType<CameraBehavior>();
        cam.enabled = true;
        if (cam.enabled == true)
        {
            disableTime -= Time.deltaTime;
            if (disableTime <= 0)
            {
                drc.spellCamera.enabled = false;
            }

        }

        drc.spellHitPointIndicator.enabled = false;

        finalPos = TerrainManager.instance.PeakPoint(drc.lightPos.x, drc.lightPos.z).point + Vector3.down * 12f;
        startPos = finalPos + Vector3.down * startPositionHeightOffset; // So it starts in the ground
        transform.position = startPos;

        //Disable(duration * 100000f);
    }

    protected void OnTriggerEnter(Collider collision) {
        if (!collisions) { return; }
        if (explosion) { Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3f); }

        if (collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponentOnObject<Player>();
            if (!playersHit.Contains(player)) {
                Vector3 playerDirection = player.transform.position - transform.position;
                player.Damage(contactDamage);
                player.GetComponent<Rigidbody>().Sleep();
                playerDirection.Normalize();
                player.rigidbody.AddForce(playerDirection.x * knockBackForce, (playerDirection.y / 6f + 1f) * knockBackForce,
                    playerDirection.z * knockBackForce);
                playersHit.Add(player);
            }
            
        }


    }
}
