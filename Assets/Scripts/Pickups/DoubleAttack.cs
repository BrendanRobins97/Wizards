using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DoubleAttack : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private TextMeshProUGUI text;
    private Player player, playerWithItem;
    [HideInInspector]public bool pickedUp = false;
    private bool doubleAttack = false;
    private int numFire = 0;
    private float tempTime;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        Debug.Log(player);
        if (pickedUp)
        {
            if (doubleAttack)
            {
                playerWithItem.enabled = true;
                playerWithItem.turnOver = false;
                gm.currentTurnTimeLeft = tempTime;
            }
            if (playerWithItem != player)
            {
                text.gameObject.SetActive(false);
            }
            if (Input.GetButtonUp("Fire1") && pickedUp && player == playerWithItem && numFire < 1)
            {
                playerWithItem.enabled = true;
                playerWithItem.turnOver = false;
                playerWithItem.playerCamera.fieldOfView = playerWithItem.originalFOV;
                text.gameObject.SetActive(false);
                numFire++;
                doubleAttack = false;
                //Destroy(this.gameObject,5f);
            }

            if (Input.GetButtonUp("Fire1") && pickedUp && player == playerWithItem && numFire >= 1)
            {
                doubleAttack = false;
                text.gameObject.SetActive(false);
                Debug.Log("ShouldReset");
                Destroy(this.gameObject, .2f);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && player == gm.GetComponent<GameManager>().CurrentPlayer)
        {
            tempTime = gm.currentTurnTimeLeft;
            playerWithItem = player;
            pickedUp = true;
            doubleAttack = true;
            text.gameObject.SetActive(true);
            text.text = "Double Attack";
            playerWithItem.numberOfAttacks++;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
