﻿// File: Player.cs
// Author: Brendan Robinson
// Date Created: 02/19/2019
// Date Last Modified: 02/28/2019

using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Constants

    private const float chargeMax = 2f;
    private const float chargeRate = 1f;
    [HideInInspector] private const float startingStamina = 30f;

    #endregion

    #region Fields

    public Color color;
    public float chargeAmount;
    
    public float stamina;
    public float jumpForce = 400;
    [HideInInspector] public int health;
    [HideInInspector] public float chargePercent;
    [HideInInspector] public float tempChargeAmount;
    [HideInInspector] public bool turnOver;

    [SerializeField] private float sensitivity = 1f;

    [HideInInspector] public float movementSpeed = 8f;
    [HideInInspector] public bool enabled = true;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] private List<Spell> spells;
    [SerializeField] private Transform feetPosition;

    [SerializeField] public Animator animator;
    [SerializeField] private GameObject PS_ElectricOrbPrefab;
    [HideInInspector] public Rigidbody rigidbody;

    //private Rigidbody   rigidbody;


    private float currentCameraRotationX;
    private readonly float cameraRotationLimit = 80f;

    private readonly float cameraDistFromPlayer = 6f;
    private readonly float cameraYOffset = 2f;
    private readonly float cameraXOffset = 1f;
    private int specialCount = 0;
    private int currentSpellIndex;
    private Vector3 prevPosition;
    private DeathRainSpellCamera drsc;
    [HideInInspector] public float originalFOV = 0f;
    [HideInInspector] public int numberOfAttacks = 1;
    [HideInInspector] public bool usedSpecial = false;
    [HideInInspector] public int numUlt = 1;
    [HideInInspector] public bool casting = false;
    [HideInInspector] public bool special = false;
    private InputControl ic;
    public GameObject soundPlay;
    #endregion

    #region Methods

    private void Awake()
    {
        ic = FindObjectOfType<InputControl>();
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        Disable();
        //locks the cursor to the bounds of the screen. Press 'esc' to unlock.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        drsc = FindObjectOfType<DeathRainSpellCamera>();
        special = false;
        originalFOV = playerCamera.fieldOfView;
    }

    private void Update() {
        if (!enabled || casting) { return; }

        // Vertical rotation calculations
        // Applies to Camera
        float xRot = Input.GetAxisRaw("Mouse Y");
        if (GameManager.instance.isController)
        {
            xRot *= -1;
        }
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
            animator.SetFloat("Strafe Amount", xVelocity*2.0f);
            animator.SetFloat("Forward Amount", Mathf.Abs(zVelocity / 2.0f));
            if (Input.GetButtonDown("Jump") && Physics.Raycast(feetPosition.position, Vector3.down, 0.5f)) {
                rigidbody.AddForce(0, jumpForce, 0);
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxis("spell1") == -1) { currentSpellIndex = 0; }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis("spell2") == 1) { currentSpellIndex = 1; }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxis("spell1") == 1) { currentSpellIndex = 2; }

        if ((Input.GetKeyDown(KeyCode.Alpha4) && !usedSpecial) || (Input.GetAxis("spell2") == -1 && numUlt > 0))
        {
            currentSpellIndex = 3;
            drsc.Activate();
            special = true;
        }
        if (currentSpellIndex == 3 && (usedSpecial || numUlt < 1))
        {
            currentSpellIndex = 0;
        }
        currentSpellIndex = Mathf.Clamp(currentSpellIndex, 0, spells.Count - 1);

        GameManager.instance.UpdateSpellImage(currentSpellIndex);

        if (Input.GetButtonUp("Fire1")) {
            casting = true;
            if (currentSpellIndex == 3 && special)
            {
                special = false;
                //animator.ResetTrigger("Idle");
                animator.SetTrigger("Cast3");
                numUlt--;
                usedSpecial = true;    
            }
            if (currentSpellIndex == 3 && !special && numberOfAttacks > 0)
            {
                //drsc.Activate();
                special = true;
            }
            if (currentSpellIndex == 0)
            {
                animator.ResetTrigger("Charge");
                animator.SetTrigger("Cast0");
            }

            if (currentSpellIndex == 1)
            {
                //animator.ResetTrigger("Idle");
                animator.SetTrigger("Cast1");
                playerCamera.fieldOfView = originalFOV;
            }

            if (currentSpellIndex == 2)
            {
                playerCamera.fieldOfView = originalFOV;
                //animator.ResetTrigger("Idle");
                animator.SetTrigger("Cast2");
            }
            
        }
        // Handle charge for spell
        if (Input.GetButton("Fire1") )
        {
            if (currentSpellIndex == 0)
            {
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Charge");
            }
            
            chargeAmount += Time.deltaTime * chargeRate;
            if (chargeAmount > .5f && chargeAmount < chargeMax)
            {
                tempChargeAmount = chargeAmount;
                playerCamera.fieldOfView -= 30f * Time.deltaTime;
            }
        }
        else
        {
            chargeAmount = 0;
        }
        
        chargeAmount = Mathf.Clamp(chargeAmount, 0, chargeMax);
        chargePercent = chargeAmount / chargeMax;
    }

    public void Cast()
    {
        numberOfAttacks--;
        casting = true;
        chargePercent = tempChargeAmount / chargeMax;
        special = false;
        // Fire spell when mouse is released
        Vector3 spellStart =
            transform.TransformPoint(new Vector3(cameraXOffset, cameraYOffset, 0.5f));
        Instantiate(spells[currentSpellIndex], spellStart, Quaternion.identity)
            .ThrowSpell(playerCamera.transform.forward, chargePercent);
        //animator.SetFloat("Forward Amount", 0);
        //animator.SetFloat("Strafe Amount", 0.0f);

        animator.SetTrigger("Idle");
        drsc.spellHitPointIndicator.enabled = false;
        Debug.Log("Num Attacks " + numberOfAttacks);
        enabled = true;
        if (numberOfAttacks <= 0) {
            enabled = false; // Disable movement
            turnOver = true; // Signal that their turn is over
        }
    }
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
        soundPlay = GameObject.Find("soundManager");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playPlayerStart();
    }

    public void Disable()
    {
        casting = false;
        turnOver = true;
        enabled = false;
        animator.ResetTrigger("Charge");
        animator.SetFloat("Forward Amount", 0);
        animator.SetFloat("Strafe Amount", 0.0f);
        animator.SetTrigger("Idle");
        Input.ResetInputAxes();
        if (playerCamera) { playerCamera.enabled = false; }
    }

    public void Damage(int amount) {
        health -= amount; 
        FloatingTextManager.instance.SpawnDamageText(transform.position + Vector3.up, amount);
        soundPlay = GameObject.Find("soundManager");
        animator.SetTrigger("Hit");
        animator.SetTrigger("Idle");
        soundScript sound = soundPlay.GetComponent(typeof(soundScript)) as soundScript;
        sound.playOof();
    }

    public void Kill()
    {
        health = 0; 
        animator.SetTrigger("Dead");
    }

    public float HealthPercent() { return (float) health / maxHealth; }

    public float StaminaPercent()
    {
        return (float) stamina / startingStamina;
    }

    #endregion

}