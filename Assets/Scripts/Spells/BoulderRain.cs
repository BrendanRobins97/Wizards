// File: BoulderRain.cs
// Contributors: Brendan Robinson
// Date Created: 04/21/2019
// Date Last Modified: 04/21/2019

using UnityEngine;

public class BoulderRain : Spell {

    #region Fields

    public float sizeIncreaseRate = 1f;
    public LayerMask groundMask;

    private float disableTime = .2f;
    private float size;
    private Vector3 prevPosition;
    private Collider collider;

    #endregion

    #region Methods

    protected override void Start() {
        base.Start();
        size = transform.localScale.x; // Get the start size
        prevPosition = Vector3.one;
    }

    private void Update() {
        Vector3 dist = prevPosition - transform.position;
        prevPosition = transform.position;
        if (Physics.CheckSphere(transform.position, size + 0.25f, groundMask)) {
            size += Time.deltaTime * sizeIncreaseRate * dist.magnitude;
        }

        transform.localScale = new Vector3(size, size, size);
        if (dist.magnitude <= 0.00001f) {
            //Disable(0.5f); // Disable when its done moving
        }
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
            player.Damage(contactDamage);
        }
    }

    #endregion

}