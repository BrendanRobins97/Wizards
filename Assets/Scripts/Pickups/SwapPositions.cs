using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPositions : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    private Player player, playerWithItem;
    [SerializeField] private Player[] players;

    private bool playersFound = false;

    private bool canSwap = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        this.GetComponent<MeshRenderer>().material.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        Debug.Log(player);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canSwap && player == playerWithItem)
            {
                ShowPlayers();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) && canSwap && player == playerWithItem)
        {
            Vector3 tempPos = player.transform.position;
            player.transform.position = players[0].transform.position;
            players[0].transform.position = tempPos;
            Destroy(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && canSwap && player == playerWithItem)
        {
            Vector3 tempPos = player.transform.position;
            player.transform.position = players[1].transform.position;
            players[1].transform.position = tempPos;
            Destroy(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && canSwap && player == playerWithItem)
        {
            Vector3 tempPos = player.transform.position;
            player.transform.position = players[2].transform.position;
            players[2].transform.position = tempPos;
            Destroy(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && canSwap && player == playerWithItem)
        {
            Vector3 tempPos = player.transform.position;
            player.transform.position = players[3].transform.position;
            players[3].transform.position = tempPos;
            Destroy(this.gameObject);
        }
        if (!playersFound)
        {
            players = FindObjectsOfType<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                print(players[i]);
            }

            playersFound = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            playerWithItem = player;
            canSwap = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void ShowPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
           print(players[i] + " index " + i);
        }
    }
}
