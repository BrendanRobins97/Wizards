using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
        if (!playersFound)
        {
            players = FindObjectsOfType<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                print(players[i]);
            }

            playersFound = true;
        }
        if (pickedUp) { 
        Debug.Log(player);
        if (gm.numPlayersLeft != players.Length)
        {
            Debug.Log("PlayerDied");
            players = FindObjectsOfType<Player>();
            //playersFound = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && canSwap && player == playerWithItem)
        {
            index++;
            if (index >= players.Length)
            {
                index = 0;
            }

            if (playerWithItem.name == players[index].name)
            {
                index++;
                if (index >= players.Length || players[index] == null)
                {
                    index = 0;
                }
            }

            swapPositionsText.text = players[index].name;
            //ShowPlayers();
        }

        if (Input.GetMouseButtonDown(1) && canSwap && player == playerWithItem)
        {
            Vector3 tempPos = player.transform.position;
            player.transform.position = players[index].transform.position;
            players[index].transform.position = tempPos;
            Debug.Log(player.name + " swapped with " + players[index].name);
            swapPositionsText.gameObject.SetActive(false);
            canSwap = false;
            Destroy(this.gameObject, .2f);
        }

        if (playerWithItem != player)
        {
            swapPositionsText.text = "";
        }

        if (playerWithItem == player && canSwap)
        {
            swapPositionsText.gameObject.SetActive(true);
            swapPositionsText.text = "Press E to Show Players." +
                                     "Right Click to Swap.";
        }
    }
}
    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        // replace the element at index with the last element
        arr[index] = arr[arr.Length - 1];
        // finally, let's decrement Array's size by one
        System.Array.Resize(ref arr, arr.Length - 1);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && player == gm.GetComponent<GameManager>().CurrentPlayer)
        {
            playerWithItem = player;
            pickedUp = true;
            canSwap = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            swapPositionsText.gameObject.SetActive(true);
            swapPositionsText.text = "Press E to Show Players." +
                                     "Right Click to Swap.";
        }
    }

    void ShowPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name != playerWithItem.name && players[i] != null)
            {
                print(players[i] + " index " + i);
                
            }

        }
    }
}
