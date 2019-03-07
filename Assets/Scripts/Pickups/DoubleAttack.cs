using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    private Player player, playerWithItem;
    private bool pickedUp;
    private bool doubleAttack = false;
    private int numFire = 0;
    private float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        this.GetComponent<MeshRenderer>().material.color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        Debug.Log(player);
        if (doubleAttack)
        {
            playerWithItem.enabled = true;
            playerWithItem.turnOver = false;
            gm.currentTurnTimeLeft = tempTime;
        }

        if (Input.GetButtonUp("Fire1")&&pickedUp&&player == playerWithItem && numFire <= 1)
        {   
            playerWithItem.enabled = true;
            playerWithItem.turnOver = false;
            Destroy(this.gameObject,.2f);
        }
        if (Input.GetButtonUp("Fire1") && pickedUp && player == playerWithItem && numFire > 1)
        {
            doubleAttack = false;
            Destroy(this.gameObject, .2f);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            tempTime = gm.currentTurnTimeLeft;
            playerWithItem = player;
            pickedUp = true;
            doubleAttack = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
