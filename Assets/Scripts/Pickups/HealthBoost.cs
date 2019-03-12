using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HealthBoost : MonoBehaviour
{
    [SerializeField] private int healthBoost = 20;
    private Player player, playerWithItem;
    [SerializeField] private TextMeshProUGUI text;
    private bool pickedUp = false;
    private float displayTime;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        displayTime -= Time.deltaTime;
        if (pickedUp)
        {
            if (displayTime <= 0 && pickedUp)
            {
                text.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }

            if (playerWithItem != player)
            {
                text.gameObject.SetActive(false);
            }

            if (player == null)
            {
                Debug.Log("No Player Found");
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            playerWithItem = player;
            Debug.Log("HealthBoost");
            player.GetComponent<Player>().health = player.GetComponent<Player>().health + healthBoost;
            text.gameObject.SetActive(true);
            text.text = "Health Boost";
            pickedUp = true;
            displayTime = 2f;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
