// File: GameManager.cs
// Author: Brendan Robinson
// Date Created: 02/12/2019
// Date Last Modified: 02/12/2019
// Description: Controlls the game logic for players. Handles who's turn it is, how much time is left, etc.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider chargeBar;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private readonly int numPlayers = 2;
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private float turnTime = 20f;

    private List<Player> players = new List<Player>();

    private int playerTurn = 0;

    private float currentTurnTimeLeft;

    private void Start()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            players.Add(Instantiate(playerPrefab, spawnLocations[i].position, spawnLocations[i].rotation, transform).GetComponent<Player>());

        }

        mainCamera.enabled = false;
        playerTurn = 0;
        players[playerTurn].Enable();
        StartTurn();
    }

    private void Update()
    {

        chargeBar.value = players[playerTurn].chargePercent;
        if (chargeBar.value <= 0)
        {
            chargeBar.enabled = false;
        }
        else
        {
            chargeBar.enabled = true;

        }
        currentTurnTimeLeft -= Time.deltaTime;
        turnText.text = "Time Left: " + (int)(currentTurnTimeLeft + 1);

        if (currentTurnTimeLeft <= 0)
        {
            NextPlayerTurn();
            StartTurn();
        }
    }

    private void NextPlayerTurn()
    {
        players[playerTurn].Disable();
        playerTurn = (playerTurn + 1) % numPlayers;
        players[playerTurn].Enable();

    }

    private void StartTurn()
    {
        currentTurnTimeLeft = turnTime;
    }
}