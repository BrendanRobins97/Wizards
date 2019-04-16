using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeedBoost : MonoBehaviour
{   
    [SerializeField]
    private float speedUpTime;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RawImage image;
    private float tempStamina;
    private GameManager gm;
    private Player player, playerWithItem;
    private bool pickedUp = false;

    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();
        //canvas.GetComponent<RawImage>().gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        //Debug.Log(player.movementSpeed);
        if (pickedUp)
        {
            if (playerWithItem != player)
            {
                image.gameObject.SetActive(false);
            }

            if (playerWithItem == player)
            {
                image.gameObject.SetActive(true);
            }

            if (pickedUp)
            {
                speedUpTime -= Time.deltaTime;
                player.GetComponent<Animator>().speed = 1.75f;
                player.GetComponent<Player>().stamina = tempStamina;
                if (speedUpTime <= 0 || gm.currentTurnTimeLeft <= 0)
                {
                    player.GetComponent<Animator>().speed = 1.0f;
                    player.movementSpeed = 2;
                    //Debug.Log("Speed normal");
                    image.gameObject.SetActive(false);
                    Destroy(this.gameObject, 0.05f);
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && player == gm.GetComponent<GameManager>().CurrentPlayer)
        {
            playerWithItem = player;
            pickedUp = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            image.gameObject.SetActive(true);
            //Debug.Log("SpeedBoost");
            player.GetComponent<Player>().movementSpeed = 4;
            tempStamina = player.GetComponent<Player>().stamina;
        }
    }
}
