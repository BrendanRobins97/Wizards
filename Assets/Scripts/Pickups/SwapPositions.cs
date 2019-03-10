using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwapPositions : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private TextMeshProUGUI swapPositionsText;
    private Player player, playerWithItem;
    [SerializeField] private Player[] players;
    private bool playersFound = false;
    private Canvas canvas;
    private bool canSwap = false;

    private int index = 0;
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
        if (Input.GetKeyDown(KeyCode.E) && canSwap && player == playerWithItem)
        {
            index++;
            if (canSwap && player == playerWithItem)
            {
                if (index >= players.Length)
                {
                    index = 0;
                }
                if (playerWithItem.name == players[index].name)
                {
                    index++;
                    if (index > players.Length)
                    {
                        index = 0;
                    }
                }
                swapPositionsText.text = players[index].name;
                ShowPlayers();
                
            }
        }

        if (Input.GetMouseButtonDown(1) && canSwap && player == playerWithItem)
        {
            Vector3 tempPos = player.transform.position;
            player.transform.position = players[index].transform.position;
            players[index].transform.position = tempPos;
            Debug.Log(player.name + " swapped with " + players[index].name);
            swapPositionsText.gameObject.SetActive(false);
            
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
            swapPositionsText.gameObject.SetActive(true);
        }
    }

    void ShowPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name != playerWithItem.name)
            {
                print(players[i] + " index " + i);
                
            }

        }
    }
}
