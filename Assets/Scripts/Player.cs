// File: Player.cs
// Contributors: Brendan Robinson
// Date Created: 05/11/2019
// Date Last Modified: 05/12/2019

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    #region Constants

    private const float chargeMax = 2f;
    private const float chargeRate = 1f;
    private const float startingStamina = 45f;

    #endregion

    #region Fields

    public string wizardName;
    public Sprite icon;
    public Color color;
    public float chargeAmount;
    public Transform hitPoint;
    public Transform demonHit1; //, demonHit2, demonHit3, demonHit4;
    public float stamina;
    public float jumpForce = 400;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public float chargePercent;
    [HideInInspector]
    public float tempChargeAmount;
    [HideInInspector]
    public bool turnOver;
    public LayerMask groundMask;
    [SerializeField]
    private float sensitivity = 1f;

    public float movementSpeed = 8f;
    [HideInInspector]
    public bool enabled = true;
    [SerializeField]
    public Camera playerCamera;
    [SerializeField]
    public int maxHealth = 100;
    [SerializeField]
    public List<Spell> spells;
    [SerializeField]
    private Transform feetPosition;
    [SerializeField]
    private LaunchArc launchArc;
    [SerializeField]
    public TextMeshProUGUI nameUI;
    [SerializeField]
    public GameObject healthBar;

    [SerializeField]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rigidbody;

    private float currentCameraRotationX;
    private readonly float cameraRotationLimit = 80f;

    private readonly float cameraDistFromPlayer = 6f;
    private readonly float cameraYOffset = 2f;
    private readonly float cameraXOffset = 1f;
    private int specialCount = 0;
    private int currentSpellIndex;
    private Vector3 prevPosition;
    private DeathRainSpellCamera drsc;
    [HideInInspector]
    public float originalFOV;
    [HideInInspector]
    public int numberOfAttacks = 1;
    [HideInInspector]
    public bool usedSpecial;
    [HideInInspector]
    public int numUlt = 1;
    [HideInInspector]
    public bool casting;
    [HideInInspector]
    public bool special;
    public GameObject soundPlay;
    [HideInInspector]
    public int index = 0;
    
    #endregion

    #region Methods

    public Spell CurrentSpell => spells[currentSpellIndex];

    private void Awake() {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        Disable();
        //locks the cursor to the bounds of the screen. Press 'esc' to unlock.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        drsc = FindObjectOfType<DeathRainSpellCamera>();
        special = false;
        originalFOV = playerCamera.fieldOfView;
        if (nameUI) {
            nameUI.text = wizardName;
            nameUI.color = color;
            nameUI.enabled = false;
        }    
    }

    private void Update() {
        launchArc.gameObject.SetActive(false);
        
        if (!enabled) { return; }

        // Vertical rotation calculations
        // Applies to Camera
        float xRot = Input.GetAxisRaw("Mouse Y");
        if (GameManager.instance.isController) { xRot *= -1; }
        float cameraRotationX = xRot * sensitivity;
        if (xRot > .078 || xRot < -.078) { currentCameraRotationX -= cameraRotationX; }
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        float cameraOffsetZ = -cameraDistFromPlayer * Mathf.Cos(currentCameraRotationX * Mathf.Deg2Rad);
        float cameraOffsetY = cameraDistFromPlayer * Mathf.Sin(currentCameraRotationX * Mathf.Deg2Rad)
                              + cameraYOffset;
        cameraOffsetY = Mathf.Max(0, cameraOffsetY);
        playerCamera.transform.localPosition = new Vector3(cameraXOffset, cameraOffsetY, cameraOffsetZ);
        playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

        // Horizontal rotation calculations
        // Applies to character
        float rot = Input.GetAxisRaw("Mouse X");
        Vector3 yRot = new Vector3(0f, rot, 0f) * sensitivity;
        if (rot > .078 || rot < -.078) { rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(yRot)); }

        stamina -= (transform.position - prevPosition).magnitude;
        prevPosition = transform.position;
        animator.SetFloat("Forward Amount", 0.0f);
        animator.SetFloat("Strafe Amount", 0.0f);

        if (stamina > 0) {
            // Movement Calculations
            float xVelocity = Input.GetAxis("Horizontal") * movementSpeed;
            float zVelocity = Input.GetAxis("Vertical") * movementSpeed;

            Vector3 movX = transform.right * xVelocity;
            Vector3 movZ = transform.forward * zVelocity;

            Vector3 velocity = (movX + movZ) * movementSpeed * Time.deltaTime;

            rigidbody.MovePosition(rigidbody.position + velocity);
            animator.SetFloat("Strafe Amount", Input.GetAxis("Horizontal"));
            animator.SetFloat("Forward Amount", Input.GetAxis("Vertical"));
            if (Input.GetButtonDown("Jump") && Physics.Raycast(feetPosition.position, Vector3.down, 0.5f)) {
                rigidbody.AddForce(0, jumpForce, 0);
            }
        }
        if (!casting) { // Cant swap spells while casting
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxis("spell1") == -1) { currentSpellIndex = 0; }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis("spell2") == 1) { currentSpellIndex = 1; }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxis("spell1") == 1) { currentSpellIndex = 2; }

            if (Input.GetKeyDown(KeyCode.Alpha4) && numUlt > 0 || Input.GetAxis("spell2") == -1 && numUlt > 0) {
                currentSpellIndex = 3;
                drsc.Activate();
                special = true;
            }
        }

        if (currentSpellIndex == 3 && numUlt < 1) { currentSpellIndex = 0; }
        currentSpellIndex = Mathf.Clamp(currentSpellIndex, 0, spells.Count - 1);

        GameManager.instance.UpdateSpellImage(currentSpellIndex);

        if (Input.GetButtonUp("Fire1")) {
            casting = false;
            if (currentSpellIndex == 3 && numUlt > 0) {
                special = false;
                animator.SetTrigger("Cast3");
                numUlt--;
                usedSpecial = true;
            }
            if (currentSpellIndex == 3 && !special && numberOfAttacks > 0) {
                //drsc.Activate();
                special = true;
            }
            if (currentSpellIndex == 0) {
                animator.ResetTrigger("Charge");
                animator.SetTrigger("Cast0");
            }

            if (currentSpellIndex == 1) {
                animator.ResetTrigger("Charge1");
                animator.SetTrigger("Cast1");
                //playerCamera.fieldOfView = originalFOV;
            }

            if (currentSpellIndex == 2) {
                playerCamera.fieldOfView = originalFOV;
                animator.SetTrigger("Cast2");
            }
        }
        // Handle charge for spell

        if (Input.GetButton("Fire1")) {
            casting = true;

            if (currentSpellIndex == 0) {
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Charge");
            }

            if (currentSpellIndex == 1) {
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Charge1");
            }
            chargeAmount += Time.deltaTime * chargeRate;
            if (currentSpellIndex == 0) {
                launchArc.gameObject.SetActive(true);
                launchArc.transform.forward = playerCamera.transform.forward;

                Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                Physics.Raycast(ray, out RaycastHit rayHit, 100f, groundMask);

                if (rayHit.collider != null) {
                    launchArc.transform.forward = rayHit.point - launchArc.transform.position;
                    launchArc.MakeArcMesh(Mathf.Clamp(chargeAmount, 0.01f, chargeMax) * CurrentSpell.speed, 0,
                        rayHit.distance);
                }
                else {
                    launchArc.transform.forward = playerCamera.transform.TransformPoint(new Vector3(0, 0, 100f)) -
                                                  launchArc.transform.position;
                    launchArc.MakeArcMesh(Mathf.Clamp(chargeAmount, 0.01f, chargeMax) * CurrentSpell.speed, 0, 100f);
                }
            }
            if (currentSpellIndex == 1) {
                launchArc.gameObject.SetActive(true);
                launchArc.MakeArcMesh(Mathf.Clamp(chargeAmount, 0.01f, chargeMax) * CurrentSpell.speed,
                    Mathf.Abs(Physics.gravity.y));
                launchArc.transform.forward = new Vector3(playerCamera.transform.forward.x,
                    playerCamera.transform.forward.y / 1.35f, playerCamera.transform.forward.z);
            }

            if (chargeAmount > .5f && chargeAmount < chargeMax) {
                tempChargeAmount = chargeAmount;
                playerCamera.fieldOfView -= 25f * Time.deltaTime;
            }
        }
        else { chargeAmount = 0; }

        chargeAmount = Mathf.Clamp(chargeAmount, 0f, chargeMax);
        chargePercent = chargeAmount / chargeMax;
    }

    public void Cast() {
        numberOfAttacks--;
        casting = true;
        chargePercent = tempChargeAmount / chargeMax;
        special = false;
        // Fire spell when mouse is released
        Vector3 spellStart =
            transform.TransformPoint(new Vector3(cameraXOffset, cameraYOffset, 0.5f));
        Instantiate(spells[currentSpellIndex], spellStart, Quaternion.identity)
            .ThrowSpell(playerCamera.transform.forward, chargePercent);
        AnimTriggerReset();
        drsc.spellHitPointIndicator.enabled = false;
        Debug.Log("Num Attacks " + numberOfAttacks);
        enabled = true;
        if (numberOfAttacks <= 0) {
            enabled = false; // Disable movement
            turnOver = true; // Signal that their turn is over
        }
    }

    // Enables the camera only without allowing movement
    public void EnableCamera() {
        playerCamera.enabled = true;
        chargeAmount = 0;
        stamina = startingStamina;
        playerCamera.fieldOfView = originalFOV;
        nameUI.enabled = false;
    }

    // Enable to start turn and allow movement/the ability to cast spell
    public void Enable() {
        turnOver = false;
        enabled = true;
        numberOfAttacks = 1;
        playerCamera.enabled = true;
        chargeAmount = 0;
        stamina = startingStamina;
        prevPosition = transform.position;
        playerCamera.fieldOfView = originalFOV;
        Input.ResetInputAxes();
        AnimTriggerReset();
        animator.SetTrigger("Idle");
        soundPlay = GameObject.Find("soundManager");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playPlayerStart(icon.name);
        nameUI.enabled = false;

    }

    // Disable movement and turn off camera
    public void Disable() {
        casting = false;
        turnOver = true;
        enabled = false;
        animator.SetFloat("Forward Amount", 0);
        animator.SetFloat("Strafe Amount", 0.0f);
        AnimTriggerReset();
        Input.ResetInputAxes();
        launchArc.gameObject.SetActive(false);
        stamina = startingStamina;
        if (playerCamera) { playerCamera.enabled = false; }
    }

    public void Damage(int amount) {
        health -= amount;
        FloatingTextManager.instance.SpawnDamageText(transform.position + Vector3.up, amount);
        soundPlay = GameObject.Find("soundManager");
        animator.SetTrigger("Hit");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playOof(icon.name);
        GameManager.instance.Damage(amount, index);
        //CameraController.instance.ScreenShake(playerCamera);
    }

    public void Kill() { health = 0; }

    public float HealthPercent() { return (float) health / maxHealth; }

    public float StaminaPercent() { return stamina / startingStamina; }

    public void AnimTriggerReset() {
        animator.ResetTrigger("Cast0");
        animator.ResetTrigger("Cast1");
        animator.ResetTrigger("Cast2");
        animator.ResetTrigger("Cast3");
        animator.ResetTrigger("Hit");
        animator.SetTrigger("Idle");
    }

    public void EnableCollider() { FindObjectOfType<HammerHit>().EnableCollider(); }

    public void EnableCollider1() { FindObjectOfType<DemonMelee>().EnableCollider(); }

    

    #endregion

}