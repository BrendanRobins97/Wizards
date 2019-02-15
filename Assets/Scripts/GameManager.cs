// File: GameManager.cs
// Author: Brendan Robinson
// Date Created: 02/15/2019
// Date Last Modified: 02/15/2019
// Description: 

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider chargeBar;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Transform playerInfoContainer;

    [SerializeField] private PlayerUI playerUIPrefab;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private int numPlayers = 2;
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private float turnTime = 20f;
    [SerializeField] private float timeAfterSpellCast = 5f;

    private List<PlayerInfo> players = new List<PlayerInfo>();

    private int playerTurn;

    private float currentTurnTimeLeft;

    private bool endOfTurn;

    public Player CurrentPlayer => players[playerTurn].player;

    private void Start()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            PlayerInfo newPlayerInfo;
            newPlayerInfo.player = Instantiate(playerPrefab, spawnLocations[i].position, spawnLocations[i].rotation,
                    transform)
                .GetComponent<Player>();
            newPlayerInfo.playerUI = Instantiate(playerUIPrefab, playerInfoContainer).GetComponent<PlayerUI>();
            newPlayerInfo.dead = false;
            players.Add(newPlayerInfo);
        }

        mainCamera.enabled = false;
        playerTurn = 0;
        CurrentPlayer.Enable();
        StartTurn();
    }

    private void Update()
    {
        chargeBar.value = CurrentPlayer.chargePercent;
        if (chargeBar.value <= 0)
        {
            chargeBar.enabled = false;
        }
        else
        {
            chargeBar.enabled = true;
        }

        currentTurnTimeLeft -= Time.deltaTime;
        currentTurnTimeLeft = Mathf.Clamp(currentTurnTimeLeft, -0.9f, turnTime);
        turnText.text = "Time Left: " + (int) (currentTurnTimeLeft + 1);

        if (CurrentPlayer.turnOver && !endOfTurn)
        {
            currentTurnTimeLeft = timeAfterSpellCast;
            endOfTurn = true;
        }

        // Destroy all players with health below 0
        for (int i = 0; i < numPlayers; i++)
        {
            Player player = players[i].player;
            
            if (player == null)
            {
                continue;
            }
            PlayerUI playerUI = players[i].playerUI;
            playerUI.healthBar.value = player.HealthPercent();
            if (player.health <= 0)
            {
                Destroy(player.gameObject);
                playerUI.playerImage.color = Color.red;
            }
        }

        if (currentTurnTimeLeft <= 0)
        {
            NextPlayerTurn();
            StartTurn();
        }
    }

    private void NextPlayerTurn()
    {
        // Disable current player
        CurrentPlayer.Disable();

        // Find the next available player
        int count = 0;
        do
        {
            playerTurn = (playerTurn + 1) % numPlayers;
            count++;
        } while (CurrentPlayer == null && count <= numPlayers);

        CurrentPlayer?.Enable();
    }

    private void StartTurn()
    {
        endOfTurn = false;
        currentTurnTimeLeft = turnTime;
    }
}

public struct PlayerInfo
{
    public Player player;
    public PlayerUI playerUI;
    public bool dead;
}