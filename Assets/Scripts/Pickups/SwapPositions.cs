using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPositions : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    private Player player;
    [SerializeField] private Player[] players;

    private bool playersFound = false;
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
            Vector3 tempPos = player.transform.position;
            player.transform.position = players[0].transform.position;
            players[0].transform.position = tempPos;
            Destroy(this.gameObject);
        }
    }
}
