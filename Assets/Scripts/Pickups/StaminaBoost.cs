﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBoost : MonoBehaviour
{
    private Player player;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        this.GetComponent<MeshRenderer>().material.color = Color.green;
    }

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
        if (player != null)
        {
            if (col.tag == "Player")
            {
                player.GetComponent<Player>().stamina = player.GetComponent<Player>().stamina + 20f;
                Destroy(this.gameObject, .02f);
            }
        }
    }
}
