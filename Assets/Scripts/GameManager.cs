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
    [SerializeField] private GameObject giantFireball;
    [SerializeField] private int              numPlayers = 2;
    [SerializeField] private List<Transform>  spawnLocations;
    [SerializeField] private float            turnTime           = 20f;
    [SerializeField] public float            timeAfterSpellCast = 5f;
    [SerializeField] public float gameStartTime = 5f;

    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private List<Image>      spellImages;

    private List<PlayerInfo> players = new List<PlayerInfo>();
    [HideInInspector] public int              playerTurn;
    public float            currentTurnTimeLeft;
    private bool             endOfTurn, newRound;
    private bool gameStarted = false;
    private float xBoundsMax,xBoundsMin, zBoundsMin, zBoundsMax;
   [HideInInspector] public int              numPlayersLeft,roundNumber,mapShrinkNumber;

    #endregion

    #region Methods

    public Player CurrentPlayer => players[playerTurn].player;

    private void Awake() {
        if (instance != null) { Destroy(this); }
        else { instance = this; }
    }

    private void Start() {
        mainMenu mm = FindObjectOfType<mainMenu>();
        if (mm) {
            numPlayers = mm.NumPlayers();
            Destroy(mm.gameObject);
        }
        mapShrinkNumber = 2;
        xBoundsMax = GameObject.FindObjectOfType<TerrainManager2>().length;
        xBoundsMin = 0;
        zBoundsMin = 0;
        zBoundsMax = GameObject.FindObjectOfType<TerrainManager2>().width;
        currentTurnTimeLeft = gameStartTime;
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
        roundNumber = 0;
        playerTurn = 0;
        numPlayersLeft = numPlayers;
    }

    private void Update() {

        // Update the current time left and update the timer text
        currentTurnTimeLeft -= Time.deltaTime;
        currentTurnTimeLeft = Mathf.Clamp(currentTurnTimeLeft, -0.9f, turnTime);
        turnText.text = "Time Left: " + (int)(currentTurnTimeLeft + 1);
        if (playerTurn == 0 && newRound && roundNumber/numPlayers > mapShrinkNumber)
        {
            MapShrink();
            newRound = false;
            mapShrinkNumber += 2;
        }
        if (playerTurn > 0)
        {
            newRound = true;
        }
        if (!gameStarted) { // Handle game start behavior
            
            // Start game when initial timer hits 0
            if (currentTurnTimeLeft < 0) {
                CurrentPlayer.Enable();
                StartTurn();
                mainCamera.enabled = false;
                gameStarted = true;
            }
        }
        
        if (!gameStarted) {
            return;
        }
        chargeBar.value = CurrentPlayer.chargePercent;
        if (chargeBar.value <= 0) { chargeBar.enabled = false; }
        else { chargeBar.enabled = true; }

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
            playerUI.staminaBar.value = player.StaminaPercent();
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
        roundNumber++;
        // Find the next available player
        int count = 0;
        do {
            playerTurn = (playerTurn + 1) % numPlayers;
            count++;
        }
        while (CurrentPlayer == null && count <= numPlayers);

        CurrentPlayer?.Enable();
    }

    public void MapShrink()
    {
        for (int x = (int)xBoundsMin; x < xBoundsMax; x += 7)
        {
            Instantiate(giantFireball,new Vector3(x,45,zBoundsMin), transform.rotation);
        }
        for (int x = (int)xBoundsMin; x < xBoundsMax; x += 7)
        {
            Instantiate(giantFireball, new Vector3(x, 45, zBoundsMax), transform.rotation);
        }
        for (int z = (int)zBoundsMin; z < zBoundsMax; z += 7)
        {
            Instantiate(giantFireball, new Vector3(xBoundsMin, 45, z), transform.rotation);
        }
        for (int z = (int)zBoundsMin; z < zBoundsMax; z += 7)
        {
            Instantiate(giantFireball, new Vector3(xBoundsMax, 45, z), transform.rotation);
        }

        zBoundsMin += 12;
        xBoundsMin += 12;
        zBoundsMax -= 12;
        xBoundsMax -= 12;
    }
    #endregion

}

public struct PlayerInfo {

    public Player   player;
    public PlayerUI playerUI;
    public bool     dead;

}