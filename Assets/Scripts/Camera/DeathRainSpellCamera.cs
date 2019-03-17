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
    [SerializeField] private GameObject spellHitPointIndicator;
    private GameObject currentSpell;

    private Vector3 forward;
    // Start is called before the first frame update
    void Start()
    {
        spellCamera = GetComponentInChildren<Camera>();
        //this.gameObject.SetActive(false);
        spellHitPointIndicator.SetActive(false);
        spellCamera.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        player = GameManager.instance.CurrentPlayer;
        

        if (Input.GetKey(KeyCode.R))
        {
            player.playerCamera.enabled = false;
            player.enabled = false;
            transform.position = new Vector3(player.playerCamera.transform.position.x, player.playerCamera.transform.position.y + 8, player.playerCamera.transform.position.z);
            forward = transform.position - new Vector3(player.transform.position.x, player.playerCamera.transform.position.y + 8,player.transform.position.z);
            transform.forward = forward;
            spellCamera.transform.LookAt(player.transform);
            startPos = transform.position;
            maxX = startPos.x + 3;
            minX = startPos.x - 3;
            maxZ = startPos.z + 3;
            minZ = startPos.z - 3;
            maxDistanceFromPlayer = 15;
            //this.gameObject.SetActive(true);
            //Cursor.lockState = CursorLockMode.None;
            spellCamera.enabled = true;
            //spellHitPointIndicator.SetActive(true);
        }
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
        this.transform.position = transform.position - velocity;
        /* if (Input.GetKey(KeyCode.A) && Mathf.Abs(Vector3.Distance(player.transform.position,transform.position))<=maxDistanceFromPlayer)
         {
             Vector3 position = this.transform.position;
             position += transform.right * Time.deltaTime * speed;
             this.transform.position = position;
         }
         if (Input.GetKey(KeyCode.D) && Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) <= maxDistanceFromPlayer)
         {
             Vector3 position = this.transform.position;
             position -= transform.right * Time.deltaTime * speed;
             this.transform.position = position;
         }
         if (Input.GetKey(KeyCode.W) && Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) <= maxDistanceFromPlayer)
         {
             Vector3 position = this.transform.position;
             position -= transform.forward * Time.deltaTime * speed;
             this.transform.position = position;
         }
         if (Input.GetKey(KeyCode.S) && Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) <= maxDistanceFromPlayer)
         {
             Vector3 position = this.transform.position;
             position += transform.forward * Time.deltaTime * speed;
             this.transform.position = position;
         }*/

    }

    private void MoveSpellIndicatorToMouse()
    {
        
        Ray ray = spellCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            lightPos = hitInfo.point;
            lightPos.y += 5;
            lightPos.z += 1;
            spellHitPointIndicator.transform.position = lightPos;
            //prefab.transform.position = hitInfo.point;
            if (Input.GetButton("Fire1") && spellCamera.enabled == true)
            {
                //this.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                //player.playerCamera.enabled = true;
                //spellCamera.enabled = false;
                player.enabled = true;
                spellHitPointIndicator.SetActive(false);
            }
        }
    }
}
