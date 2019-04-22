using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRainSpellCamera : MonoBehaviour
{
    public Camera spellCamera;
    [SerializeField] private Player player;
    private Vector3 startPos;
    private float maxX, minX, maxZ, minZ, maxDistanceFromPlayer;
    public float speed;
    public GameObject prefab;
    [HideInInspector] public Vector3 lightPos;
    [SerializeField] public Light spellHitPointIndicator;
    private GameObject currentSpell;
    public bool canShoot = false;
    private Vector3 forward;

    private float defaultFov;
    // Start is called before the first frame update
    void Start()
    {
        spellCamera = GetComponentInChildren<Camera>();
        //this.gameObject.SetActive(false);
        spellHitPointIndicator.GetComponent<Light>().enabled = false;
        spellCamera.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        player = GameManager.instance.CurrentPlayer;
        
        if (spellHitPointIndicator != null)
        {
            if (spellCamera.enabled)
            {
                MoveSpellIndicatorToMouse();
            }
        }
        float xVelocity = Input.GetAxis("Horizontal") * speed;
        float zVelocity = Input.GetAxis("Vertical") * speed;

        Vector3 movX = transform.right * xVelocity;
        Vector3 movZ = transform.forward * zVelocity;

        Vector3 velocity = (movX + movZ) * speed * Time.deltaTime;
        if (Mathf.Abs(Vector3.Distance(transform.position - velocity, player.transform.position)) < maxDistanceFromPlayer)
        {
            this.transform.position = transform.position - velocity;
        }
        else
        {
            //this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.playerCamera.transform.position.y + 8, player.transform.position.z), .500f * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxis("spell1") == -1 || Input.GetAxis("spell2") == 1 || Input.GetAxis("spell1") == 1)
        {
            player.playerCamera.enabled = true;
            player.enabled = true;
            spellHitPointIndicator.enabled = false;
            spellCamera.enabled = false;
            player.special = false;
        }

    }

    public void Activate()
    {
        player.playerCamera.enabled = false;
        player.enabled = false;
        transform.position = new Vector3(player.playerCamera.transform.position.x, player.playerCamera.transform.position.y + 8, player.playerCamera.transform.position.z);
        forward = transform.position - new Vector3(player.transform.position.x, player.playerCamera.transform.position.y + 8, player.transform.position.z);
        transform.forward = forward;
        spellCamera.transform.LookAt(player.transform);
        startPos = transform.position;
        defaultFov = spellCamera.fieldOfView;
        maxDistanceFromPlayer = 35;
        //this.gameObject.SetActive(true);
        //Cursor.lockState = CursorLockMode.None;
        canShoot = true;
        spellCamera.enabled = true;
        spellHitPointIndicator.enabled = true;
    }
    private void MoveSpellIndicatorToMouse()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Vector3 newY = new Vector3(transform.position.x, transform.position.y - Input.GetAxis("Mouse ScrollWheel")*Time.deltaTime * 120f, transform.position.z);
            //spellCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel")*700*Time.deltaTime;
            transform.position = newY;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //spellCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 700*Time.deltaTime;
            Vector3 newY = new Vector3(transform.position.x, transform.position.y - Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 120f, transform.position.z);
            transform.position = newY;
        }

        if (Input.GetAxis("Mouse Y") > 0)
        {
            //spellCamera.fieldOfView -= Input.GetAxis("Mouse Y")* 50 * Time.deltaTime;
            Vector3 newY = new Vector3(transform.position.x, transform.position.y - Input.GetAxis("Mouse Y") * Time.deltaTime * 10f, transform.position.z);
            if (Input.GetAxis("Mouse Y") > .3 || Input.GetAxis("Mouse Y") < -.3)
                transform.position = newY;
        }
        if (Input.GetAxis("Mouse Y") < 0)
        {
            //spellCamera.fieldOfView -= Input.GetAxis("Mouse Y") * 50 * Time.deltaTime;
            Vector3 newY = new Vector3(transform.position.x, transform.position.y - Input.GetAxis("Mouse Y") * Time.deltaTime * 10f, transform.position.z);
            if (Input.GetAxis("Mouse Y") > .3 || Input.GetAxis("Mouse Y") < -.3)
                transform.position = newY;
        }
        Ray ray = spellCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            lightPos = hitInfo.point;
            lightPos.y += 5;
            //lightPos.z -= 1;
            spellHitPointIndicator.transform.position = lightPos;
            //prefab.transform.position = hitInfo.point;
            if (lightPos.y - hitInfo.point.y > 5)
            {
                //Debug.Log("Too close to camera");
                float newZoom = Mathf.Lerp(defaultFov, defaultFov + 50, 5);
                // spellCamera.fieldOfView = newZoom;
            }
            if ((Input.GetButtonUp("Fire1")) && spellCamera.enabled == true)
            {
                //this.gameObject.SetActive(false);
                player.animator.SetTrigger("Cast3");
                Cursor.lockState = CursorLockMode.Locked;
                spellHitPointIndicator.enabled = false;
                player.playerCamera.enabled = true;
                spellCamera.enabled = false;
                player.enabled = true;
                
                player.special = false;
                player.usedSpecial = true;
                player.numUlt--;
                if (player.numUlt < 0)
                {player.numUlt = 0;}
                player.Cast();
            }
        }
    }
}
