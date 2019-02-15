// File: Player.cs
// Author: Brendan Robinson
// Date Created: 02/12/2019
// Date Last Modified: 02/13/2019
// Description: 

using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float chargeMax = 2f;
    private const float chargeRate = 1f;

    public float chargeAmount;
    [HideInInspector] public int health;
    [HideInInspector] public float chargePercent;

    [SerializeField] private float sensitivity = 1f;

    [SerializeField] private float movementSpeed = 8f;
    private bool enabled = true;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private List<Spell> spells;
    private Rigidbody rigidbody;

    private float currentCameraRotationX;
    private readonly float cameraRotationLimit = 80f;

    private readonly float cameraDistFromPlayer = 6f;
    private readonly float cameraYOffset = 1f;
    private readonly float cameraXOffset = 1f;

    private int currentSpellIndex;
    [HideInInspector] public bool turnOver;

    public void Enable()
    {
        turnOver = false;
        enabled = true;
        playerCamera.enabled = true;
    }

    public void Disable()
    {
        turnOver = true;
        enabled = false;
        if (playerCamera)
        {
            playerCamera.enabled = false;
        }
    }

    private void Awake()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        Disable();
    }

    private void Update()
    {
        if (!enabled)
        {
            return;
        }

        // Movement Calculations
        float xVelocity = Input.GetAxis("Horizontal") * movementSpeed;
        float zVelocity = Input.GetAxis("Vertical") * movementSpeed;

        Vector3 movX = transform.right * xVelocity;
        Vector3 movZ = transform.forward * zVelocity;

        Vector3 velocity = (movX + movZ) * movementSpeed * Time.deltaTime;

        rigidbody.MovePosition(rigidbody.position + velocity);

        // Horizontal rotation calculations
        // Applies to character
        float rot = Input.GetAxisRaw("Mouse X");

        Vector3 yRot = new Vector3(0f, rot, 0f) * sensitivity;

        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(yRot));

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSpellIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSpellIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSpellIndex = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentSpellIndex = 3;
        }

        currentSpellIndex = Mathf.Clamp(currentSpellIndex, 0, spells.Count);

        if (Input.GetButtonUp("Fire1"))
        {
            // Fire spell when mouse is released
            Vector3 spellStart =
                transform.TransformPoint(new Vector3(cameraXOffset, cameraYOffset));
            spells[currentSpellIndex].ThrowSpell(spellStart, playerCamera.transform.forward, chargePercent);
            enabled = false; // Disable movement
            turnOver = true; // Signal that their turn is over
        }
        // Handle charge for spell
        if (Input.GetButton("Fire1"))
        {
            chargeAmount += Time.deltaTime * chargeRate;
        }
        else
        {
            chargeAmount = 0;
        }

        chargeAmount = Mathf.Clamp(chargeAmount, 0, chargeMax);
        chargePercent = chargeAmount / chargeMax;
        
    }

    public void Damage(int amount)
    {
        health -= amount;
    }

    public void Kill()
    {
        health = 0;
    }

    public float HealthPercent()
    {
        return (float)health / maxHealth;
    }
}