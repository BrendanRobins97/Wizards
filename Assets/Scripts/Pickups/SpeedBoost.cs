using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpeedBoost : MonoBehaviour
{   
    [SerializeField]
    private float speedUpTime;
    [SerializeField] private TextMeshProUGUI text;
    private float tempStamina;
    private GameManager gm;
    private Player player, playerWithItem;
    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        Debug.Log(player.movementSpeed);
        if (pickedUp)
        {
            if (playerWithItem != player)
            {
                text.gameObject.SetActive(false);
            }

            if (playerWithItem == player)
            {
                text.gameObject.SetActive(true);
                text.text = "Speed Boost";
            }

            if (pickedUp)
            {
                speedUpTime -= Time.deltaTime;
                player.GetComponent<Player>().stamina = tempStamina;
                if (speedUpTime <= 0 || gm.currentTurnTimeLeft <= 0)
                {
                    player.movementSpeed = 2;
                    Debug.Log("Speed normal");
                    text.gameObject.SetActive(false);
                    Destroy(this.gameObject, 0.05f);
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            playerWithItem = player;
            pickedUp = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            text.gameObject.SetActive(true);
            text.text = "Speed Boost";
            Debug.Log("SpeedBoost");
            player.GetComponent<Player>().movementSpeed = 4;
            tempStamina = player.GetComponent<Player>().stamina;
        }
    }
}
