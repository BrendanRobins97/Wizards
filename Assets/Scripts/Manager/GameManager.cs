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
    private bool             endOfTurn, newRound, meteorShower;
    private bool gameStarted = false;
    private float circleRadius;
    private Vector3 circleCenter;
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
        mapShrinkNumber = 1;
        circleCenter = new Vector3(TerrainManager2.instance.width/2.0f,0,TerrainManager2.instance.length/2.0f);
        circleRadius = (TerrainManager2.instance.width / 2.0f) - 5.0f;
        currentTurnTimeLeft = gameStartTime;
        for (int i = 0; i < numPlayers; i++) {
            PlayerInfo newPlayerInfo;
            Ray ray = new Ray(spawnLocations[i].position, Vector3.down);
            Physics.Raycast(ray, out RaycastHit rayHit);
            Player player = Instantiate(playerPrefabs[i], rayHit.point, spawnLocations[i].rotation,
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
        if (playerTurn == 0 && newRound && roundNumber/numPlayers > mapShrinkNumber )
        {
            if (meteorShower == false)
            {
                turnTime += 5f;
                currentTurnTimeLeft = turnTime;
                meteorShower = true;
                mainCamera.enabled = true;
                MapShrink();
            }
            
            if (currentTurnTimeLeft <= 20.7f)
            {
                mapShrinkNumber += 2;
                turnTime = 20;
                mainCamera.enabled = false;
                meteorShower = false;
                newRound = false;
            }
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

        FindObjectOfType<DeathRainSpellCamera>().spellHitPointIndicator.enabled = false;
        CurrentPlayer?.Enable();
    }

    public void MapShrink()
    {
        for (int i = 0; i < 360; i+=22)
        {
            int randomY = Random.Range(23, 70);
            float ang = i;//Random.value * 360;
            Vector3 pos;
            pos.x = circleCenter.x + circleRadius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.z = circleCenter.z + circleRadius * Mathf.Cos(ang * Mathf.Deg2Rad);
            pos.y = circleCenter.y + randomY;
            Instantiate(giantFireball, pos, Quaternion.identity);
        }

        circleRadius -= 7f;
    }
    #endregion

}

public struct PlayerInfo {

    public Player   player;
    public PlayerUI playerUI;
    public bool     dead;

}