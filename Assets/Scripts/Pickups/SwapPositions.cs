using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPositions : MonoBehaviour
{
    [SerializeField] private GameManager gm;

    [SerializeField] private Player[] players;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gm.CurrentPlayer);
        players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            print(players[i]);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
          
        }
    }
}
