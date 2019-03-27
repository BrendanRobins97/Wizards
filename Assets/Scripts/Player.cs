// File: Player.cs
// Contributors: Brendan Robinson
// Date Created: 03/26/2019
// Date Last Modified: 03/26/2019

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    #region Constants

    private const                   float chargeMax       = 2f;
    private const                   float chargeRate      = 1f;
    [HideInInspector] private const float startingStamina = 30f;

    #endregion

    #region Fields

    public Color color;
    public float chargeAmount;

    public                   float stamina;
    public                   float jumpForce = 400;
    public                   bool  special;
    [HideInInspector] public int   health;
    [HideInInspector] public float chargePercent;
    [HideInInspector] public bool  turnOver;

    [HideInInspector] public float     movementSpeed = 8f;
    [HideInInspector] public bool      enabled       = true;
    [SerializeField]  public Camera    playerCamera;
    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public float     originalFOV;

    [SerializeField] private float       sensitivity = 1f;
    [SerializeField] private int         maxHealth   = 100;
    [SerializeField] private List<Spell> spells;
    [SerializeField] private Transform   feetPosition;

    [SerializeField] private GameObject PS_ElectricOrbPrefab;

    //private Rigidbody   rigidbody;

    private          float currentCameraRotationX;
    private readonly float cameraRotationLimit = 80f;

    private readonly float                cameraDistFromPlayer = 6f;
    private readonly float                cameraYOffset        = 2f;
    private readonly float                cameraXOffset        = 1f;
    private          int                  specialCount         = 0;
    private          int                  currentSpellIndex;
    private bool inLobby = true;
    private          Vector3              prevPosition;
    private          DeathRainSpellCamera drsc;

    #endregion

    #region Methods

    private void Awake() {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        //locks the cursor to the bounds of the screen. Press 'esc' to unlock.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        drsc = FindObjectOfType<DeathRainSpellCamera>();
        special = false;
        originalFOV = playerCamera.fieldOfView;
        inLobby = true; // start in lobby
        Disable();

        Enable();
    }

    private void Update() {
        if (!isLocalPlayer) { return; }
        if (!enabled) { return; }

        // Vertical rotation calculations
        // Applies to Camera
        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRot * sensitivity;

        currentCameraRotationX -= cameraRotationX;
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

        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(yRot));

        stamina -= (transform.position - prevPosition).magnitude;
        prevPosition = transform.position;

        // Don't worry about stamina if in lobby
        if (inLobby || stamina > 0) {
            // Movement Calculations
            float xVelocity = Input.GetAxis("Horizontal") * movementSpeed;
            float zVelocity = Input.GetAxis("Vertical") * movementSpeed;

            Vector3 movX = transform.right * xVelocity;
            Vector3 movZ = transform.forward * zVelocity;

            Vector3 velocity = (movX + movZ) * movementSpeed * Time.deltaTime;

            rigidbody.MovePosition(rigidbody.position + velocity);

            // Check if feet are within small distance to ground
            if (Input.GetButtonDown("Jump") && Physics.Raycast(feetPosition.position, Vector3.down, 0.5f)) {
                rigidbody.AddForce(0, jumpForce, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentSpellIndex = 0; }

        if (Input.GetKeyDown(KeyCode.Alpha2)) { currentSpellIndex = 1; }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { currentSpellIndex = 2; }

        if (Input.GetKeyDown(KeyCode.Alpha4)) { currentSpellIndex = 3; }

        currentSpellIndex = Mathf.Clamp(currentSpellIndex, 0, spells.Count - 1);

        GameManager.instance?.UpdateSpellImage(currentSpellIndex);

        if (Input.GetButtonUp("Fire1")) {
            if (currentSpellIndex == 3 && !special) {
                drsc.Activate();
                special = true;
            }
            else {
                special = false;
                // Fire spell when mouse is released
                Vector3 spellStart =
                    transform.TransformPoint(new Vector3(cameraXOffset, cameraYOffset, 0.5f));
                Spell spell = Instantiate(spells[currentSpellIndex], spellStart, Quaternion.identity);
                spell.ThrowSpell(playerCamera.transform.forward, chargePercent);
                NetworkServer.Spawn(spell.gameObject);

                if (!inLobby) {
                    enabled = false; // Disable movement
                    turnOver = true; // Signal that their turn is over
                }
            }
        }
        // Handle charge for spell
        if (Input.GetButton("Fire1")) {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            scroll = 1;
            chargeAmount += Time.deltaTime * chargeRate;
            if (chargeAmount > .5f && chargeAmount < chargeMax) {
                playerCamera.fieldOfView -= scroll * .5f;
            }
        }
        else {
            chargeAmount = 0;
            playerCamera.fieldOfView = originalFOV;

        }

        chargeAmount = Mathf.Clamp(chargeAmount, 0, chargeMax);
        chargePercent = chargeAmount / chargeMax;
    }

    public void Enable() {
        turnOver = false;
        enabled = true;
        playerCamera.enabled = true;
        chargeAmount = 0;
        stamina = startingStamina;
        prevPosition = transform.position;
        playerCamera.fieldOfView = originalFOV;
    }

    public void Disable() {
        turnOver = true;
        enabled = false;
        if (playerCamera) { playerCamera.enabled = false; }
    }

    public void Damage(int amount) { health -= amount; }

    public void Kill() { health = 0; }

    public float HealthPercent() { return (float) health / maxHealth; }

    public float StaminaPercent() { return stamina / startingStamina; }

    public void StartGame() {
        inLobby = false;
        Disable();
    }
    #endregion

}