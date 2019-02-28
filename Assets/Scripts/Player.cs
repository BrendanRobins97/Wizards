﻿// File: Player.cs
// Author: Brendan Robinson
// Date Created: 02/19/2019
// Date Last Modified: 02/28/2019

using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Constants

    private const                   float chargeMax       = 2f;
    private const                   float chargeRate      = 1f;
    [HideInInspector] private const float startingStamina = 30f;

    #endregion

    #region Fields

    public Color color;
    public float chargeAmount;

    public                   float stamina;
    [HideInInspector] public int   health;
    [HideInInspector] public float chargePercent;
    [HideInInspector] public bool  turnOver;

    [SerializeField] private float sensitivity = 1f;

    [SerializeField] private float       movementSpeed = 8f;
    private                  bool        enabled       = true;
    [SerializeField] private Camera      playerCamera;
    [SerializeField] private int         maxHealth = 100;
    [SerializeField] private List<Spell> spells;
<<<<<<< HEAD
    [SerializeField] private GameObject PS_ElectricOrbPrefab;
    private Rigidbody rigidbody;
=======
    private                  Rigidbody   rigidbody;
>>>>>>> f3320eac012ca2111a202cae93a6d7ccff1e68a3

    private          float currentCameraRotationX;
    private readonly float cameraRotationLimit = 80f;

    private readonly float cameraDistFromPlayer = 6f;
    private readonly float cameraYOffset        = 2f;
    private readonly float cameraXOffset        = 1f;

    private int     currentSpellIndex;
    private Vector3 prevPosition;

    #endregion

    #region Methods

    private void Awake() {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        Disable();
        //locks the cursor to the bounds of the screen. Press 'esc' to unlock.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update() {
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

        if (!enabled) { return; }
        stamina -= (transform.position - prevPosition).magnitude;
        prevPosition = transform.position;
        if (stamina > 0) {
            // Movement Calculations
            float xVelocity = Input.GetAxis("Horizontal") * movementSpeed;
            float zVelocity = Input.GetAxis("Vertical") * movementSpeed;

            Vector3 movX = transform.right * xVelocity;
            Vector3 movZ = transform.forward * zVelocity;

            Vector3 velocity = (movX + movZ) * movementSpeed * Time.deltaTime;

            rigidbody.MovePosition(rigidbody.position + velocity);
        }

<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSpellIndex = 0;

            PS_ElectricOrbPrefab.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSpellIndex = 1;

            PS_ElectricOrbPrefab.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSpellIndex = 2;

            PS_ElectricOrbPrefab.SetActive(false);
        }
=======
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentSpellIndex = 0; }

        if (Input.GetKeyDown(KeyCode.Alpha2)) { currentSpellIndex = 1; }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { currentSpellIndex = 2; }
>>>>>>> f3320eac012ca2111a202cae93a6d7ccff1e68a3

        if (Input.GetKeyDown(KeyCode.Alpha4)) { currentSpellIndex = 3; }

        currentSpellIndex = Mathf.Clamp(currentSpellIndex, 0, spells.Count - 1);

        GameManager.instance.UpdateSpellImage(currentSpellIndex);

        if (Input.GetButtonUp("Fire1")) {
            // Fire spell when mouse is released
            Vector3 spellStart =
                transform.TransformPoint(new Vector3(cameraXOffset, cameraYOffset, 0.5f));
            spells[currentSpellIndex].ThrowSpell(spellStart, playerCamera.transform.forward, chargePercent);
            enabled = false; // Disable movement
            turnOver = true; // Signal that their turn is over
        }
        // Handle charge for spell
        if (Input.GetButton("Fire1")) { chargeAmount += Time.deltaTime * chargeRate; }
        else { chargeAmount = 0; }

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
    }

    public void Disable() {
        turnOver = true;
        enabled = false;
        if (playerCamera) { playerCamera.enabled = false; }
    }

    public void Damage(int amount) { health -= amount; }

    public void Kill() { health = 0; }

    public float HealthPercent() { return (float) health / maxHealth; }

    #endregion

}