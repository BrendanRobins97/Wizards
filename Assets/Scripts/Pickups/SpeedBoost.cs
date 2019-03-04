using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{   
    [SerializeField]
    private float speedUpTime;
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
        if (pickedUp)
        {
            speedUpTime -= Time.deltaTime;
            if (speedUpTime < 0)
            {
                //player.movementSpeed = 8;
                Debug.Log("Speed normal");
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
            Debug.Log("SpeedBoost");
            //player.GetComponent<Player>().movementSpeed *= 2;
        }
    }
}
