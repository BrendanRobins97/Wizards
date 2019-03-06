using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [SerializeField] private int healthBoost = 20;
    private Player player;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        this.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        
        if (player == null)
        {
            Debug.Log("No Player Found");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        { 
            Debug.Log("HealthBoost");
            player.GetComponent<Player>().health = player.GetComponent<Player>().health + healthBoost;
            Destroy(this.gameObject,.05f);
        }
    }
}
