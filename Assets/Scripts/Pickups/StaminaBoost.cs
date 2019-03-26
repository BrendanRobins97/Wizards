using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StaminaBoost : MonoBehaviour
{
    private Player player,playerWithItem;
    [SerializeField] private TextMeshProUGUI text;
    private GameManager gm;
    private float displayTime = 2f;

    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        displayTime -= Time.deltaTime;
        if (pickedUp)
        {
            if (displayTime <= 0)
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
        if (player != null)
        {
            if (col.tag == "Player" && player == gm.GetComponent<GameManager>().CurrentPlayer)
            {
                playerWithItem = player;
                text.gameObject.SetActive(true);
                text.text = "Stamina Boost";
                pickedUp = true;
                displayTime = 2f;
                player.GetComponent<Player>().stamina = player.GetComponent<Player>().stamina + 20f;
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
