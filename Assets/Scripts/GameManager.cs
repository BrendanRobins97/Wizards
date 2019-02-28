// File: GameManager.cs
// Author: Brendan Robinson
// Date Created: 02/19/2019
// Date Last Modified: 02/28/2019

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Constants

    public static GameManager instance;

    #endregion

    #region Fields

    [SerializeField] private TextMeshProUGUI  gameOverText;
    [SerializeField] private Slider           chargeBar;
    [SerializeField] private TextMeshProUGUI  turnText;
    [SerializeField] private Transform        playerInfoContainer;
    [SerializeField] private PlayerUI         playerUIPrefab;
    [SerializeField] private Camera           mainCamera;
    [SerializeField] private GameObject       playerContainer;
    [SerializeField] private int              numPlayers = 2;
    [SerializeField] private List<Transform>  spawnLocations;
    [SerializeField] private float            turnTime           = 20f;
    [SerializeField] private float            timeAfterSpellCast = 5f;
    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private List<Image>      spellImages;

    private List<PlayerInfo> players = new List<PlayerInfo>();
    private int              playerTurn;
    private float            currentTurnTimeLeft;
    private bool             endOfTurn;
    private int              numPlayersLeft;

    #endregion

    #region Methods

    public Player CurrentPlayer => players[playerTurn].player;

    private void Awake() {
        if (instance != null) { Destroy(this); }
        else { instance = this; }
    }

    private void Start() {
        for (int i = 0; i < numPlayers; i++) {
            PlayerInfo newPlayerInfo;
            Player player = Instantiate(playerPrefabs[i], spawnLocations[i].position, spawnLocations[i].rotation,
                    transform)
                .GetComponent<Player>();
            PlayerUI playerUI = Instantiate(playerUIPrefab, playerInfoContainer).GetComponent<PlayerUI>();
            playerUI.playerImage.color = player.color;
            newPlayerInfo.player = player;
            newPlayerInfo.playerUI = playerUI;
            newPlayerInfo.dead = false;
            players.Add(newPlayerInfo);
        }
        gameOverText.gameObject.SetActive(false);
        mainCamera.enabled = false;
        playerTurn = 0;
        numPlayersLeft = numPlayers;
        CurrentPlayer.Enable();
        StartTurn();
    }

    private void Update() {
        chargeBar.value = CurrentPlayer.chargePercent;
        if (chargeBar.value <= 0) { chargeBar.enabled = false; }
        else { chargeBar.enabled = true; }

        currentTurnTimeLeft -= Time.deltaTime;
        currentTurnTimeLeft = Mathf.Clamp(currentTurnTimeLeft, -0.9f, turnTime);
        turnText.text = "Time Left: " + (int) (currentTurnTimeLeft + 1);

        if (CurrentPlayer.turnOver && !endOfTurn) {
            currentTurnTimeLeft = timeAfterSpellCast;
            endOfTurn = true;
        }

        // Destroy all players with health below 0
        for (int i = 0; i < numPlayers; i++) {
            Player player = players[i].player;

            if (player == null) { continue; }
            PlayerUI playerUI = players[i].playerUI;
            playerUI.healthBar.value = player.HealthPercent();
            if (player.health <= 0) {
                player.Disable();
                Destroy(player.gameObject);
                playerUI.playerImage.color = Color.gray;
                numPlayersLeft--;
                if (i == playerTurn) { currentTurnTimeLeft = 0; }
            }
        }
        if (currentTurnTimeLeft <= 0) {
            NextPlayerTurn();
            StartTurn();
        }

        if (numPlayersLeft <= 1) {
            gameOverText.text = "Player " + (playerTurn + 1) + " Wins!";
            gameOverText.gameObject.SetActive(true);
        }
    }

    public void UpdateSpellImage(int index) {
        for (int i = 0; i < spellImages.Count; i++) {
            spellImages[i].color =
                new Color(spellImages[i].color.r, spellImages[i].color.g, spellImages[i].color.b, 0.1f);
        }
        spellImages[index].color = new Color(spellImages[index].color.r, spellImages[index].color.g,
            spellImages[index].color.b, 1);
    }

    private void StartTurn() {
        endOfTurn = false;
        currentTurnTimeLeft = turnTime;
    }

    private void NextPlayerTurn() {
        // Disable current player
        CurrentPlayer.Disable();

        // Find the next available player
        int count = 0;
        do {
            playerTurn = (playerTurn + 1) % numPlayers;
            count++;
        }
        while (CurrentPlayer == null && count <= numPlayers);

        CurrentPlayer?.Enable();
    }

    #endregion

}

public struct PlayerInfo {

    public Player   player;
    public PlayerUI playerUI;
    public bool     dead;

}