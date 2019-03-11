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
    private Player player;
    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        this.GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        Debug.Log(player.movementSpeed);
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

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            pickedUp = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            text.gameObject.SetActive(true);
            text.text = "Speed Boost";
            Debug.Log("SpeedBoost");
            player.GetComponent<Player>().movementSpeed *= 2;
            tempStamina = player.GetComponent<Player>().stamina;
        }
    }
}
