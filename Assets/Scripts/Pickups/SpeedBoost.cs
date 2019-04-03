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
        Debug.Log(player.movementSpeed);
        if (pickedUp)
        {
            if (playerWithItem != player)
            {
                //text.gameObject.SetActive(false);
                //canvas.GetComponent<RawImage>().gameObject.SetActive(false);
                image.gameObject.SetActive(false);
            }

            if (playerWithItem == player)
            {
                //canvas.GetComponent<RawImage>().gameObject.SetActive(true);
                //text.gameObject.SetActive(true);
                //text.text = "Speed Boost";
                image.gameObject.SetActive(true);
            }

            if (pickedUp)
            {
                speedUpTime -= Time.deltaTime;
                //player.GetComponent<Animator>().speed = 2.0f;
                player.GetComponent<Player>().stamina = tempStamina;
                if (speedUpTime <= 0 || gm.currentTurnTimeLeft <= 0)
                {
                    player.GetComponent<Animator>().speed = 1.0f;
                    player.movementSpeed = 2;
                    Debug.Log("Speed normal");
                    //text.gameObject.SetActive(false);
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
            //text.gameObject.SetActive(true);
            //text.text = "Speed Boost";
            image.gameObject.SetActive(true);
            Debug.Log("SpeedBoost");
            player.GetComponent<Player>().movementSpeed = 4;
            tempStamina = player.GetComponent<Player>().stamina;
        }
    }
}
