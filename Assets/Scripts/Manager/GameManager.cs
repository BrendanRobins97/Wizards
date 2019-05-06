// File: GameManager.cs
// Contributors: Brendan Robinson
// Date Created: 04/11/2019
// Date Last Modified: 04/11/2019

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Constants

    public static GameManager instance;

    #endregion

    #region Fields

    [HideInInspector]
    public int playerTurn;
    [HideInInspector]
    public int numPlayersLeft, roundNumber, mapShrinkNumber;

    [Header("UI Components")]
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private TextMeshProUGUI turnText;
    [SerializeField]
    private Transform playerInfoContainer;
    [SerializeField]
    private PlayerUI playerUIPrefab;

    [Header("Prefabs")]
    [SerializeField]
    private List<GameObject> playerPrefabs;
    [SerializeField]
    private List<Image> spellImages;
    [SerializeField]
    private GameObject giantFireball;

    [Header("Game Settings")]
    public float currentTurnTimeLeft;
    public float timeAfterSpellCast = 5f;
    public float gameStartTime = 5f;
    [SerializeField]
    private float spawnWidth = 40f;
    [SerializeField]
    private float turnTime = 20f;
    [SerializeField]
    private int numPlayers;
    [SerializeField]
    private List<Transform> spawnLocations;

    private float resetGameTime = 7f;
    [Space]
    public bool isController = false;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject playerContainer;

    private List<PlayerInfo> players = new List<PlayerInfo>();
    private bool endOfTurn, newRound, meteorShower;
    private bool gameStarted;
    private float circleRadius;
    private Vector3 circleCenter;

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

        PlayerSelect ps = FindObjectOfType<PlayerSelect>();
        if (ps)
        {
            numPlayers = ps.numPlayers;
        }
        mapShrinkNumber = 1;
        circleCenter = new Vector3(TerrainManager.instance.width / 2.0f, 0, TerrainManager.instance.length / 2.0f);
        circleRadius = TerrainManager.instance.width / 2.0f - 5.0f;
        currentTurnTimeLeft = gameStartTime;
        for (int i = 0; i < numPlayers; i++) {
            PlayerInfo newPlayerInfo;
            float angle = i * 360f / numPlayers;
            Vector3 spawnLocation = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * spawnWidth + circleCenter.x, TerrainManager.instance.height, Mathf.Sin(Mathf.Deg2Rad * angle) * spawnWidth + circleCenter.z);
            Debug.Log(spawnLocation);
            Ray ray = new Ray(spawnLocation, Vector3.down);
            Physics.Raycast(ray, out RaycastHit rayHit);
            Debug.Log(rayHit.point);

            Player player;
            if (ps)
            {
                player = Instantiate(playerPrefabs[ps.playersPicked[i]], rayHit.point,
                        Quaternion.identity,
                        transform)
                    .GetComponent<Player>();
            }
            else
            {
                player = Instantiate(playerPrefabs[i], rayHit.point,
                        Quaternion.identity,
                        transform)
                    .GetComponent<Player>();
            }
            player.transform.LookAt(new Vector3(TerrainManager.instance.width / 2f, rayHit.point.y, TerrainManager.instance.length / 2f));
            PlayerUI playerUI = Instantiate(playerUIPrefab, playerInfoContainer).GetComponent<PlayerUI>();
            playerUI.playerImage.sprite = player.icon;
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
        if (ps)
        {
            Destroy(ps.gameObject);
        }
        for (int i = 0; i < CurrentPlayer.spells.Count; i++) {
            spellImages[i].sprite = CurrentPlayer.spells[i].spellImage;
            spellImages[i].color = CurrentPlayer.spells[i].spellImageColor;

        }
    }

    private void Update() {
        // Update the current time left and update the timer text
        currentTurnTimeLeft -= Time.deltaTime;
        currentTurnTimeLeft = Mathf.Clamp(currentTurnTimeLeft, -0.9f, turnTime);
        turnText.text = "Time Left: " + (int) (currentTurnTimeLeft + 1);
        if (playerTurn == 0 && newRound && roundNumber / numPlayers > mapShrinkNumber) {
            if (meteorShower == false) {
                turnTime += 5f;
                currentTurnTimeLeft = turnTime;
                meteorShower = true;
                mainCamera.enabled = true;
                MapShrink();
            }

            if (currentTurnTimeLeft <= 20.7f) {
                CurrentPlayer.enabled = true;
                FindObjectOfType<Canvas>().enabled = true;
                mapShrinkNumber += 2;
                turnTime = 20;
                mainCamera.enabled = false;
                meteorShower = false;
                newRound = false;
            }
        }
        if (playerTurn > 0) { newRound = true; }
        if(mainCamera.enabled == false) { FindObjectOfType<Canvas>().enabled = true; }
        if (!gameStarted) { // Handle game start behavior
            FindObjectOfType<Canvas>().enabled = false;
            // Start game when initial timer hits 0
            if (currentTurnTimeLeft < 0) {
                CurrentPlayer.Enable();
                StartTurn();
                mainCamera.enabled = false;
                gameStarted = true;
                FindObjectOfType<Canvas>().enabled = true;
            }
        }

        if (!gameStarted) { return; }

        if (CurrentPlayer.turnOver && !endOfTurn) {
            currentTurnTimeLeft = CurrentPlayer.CurrentSpell.timeAfterSpellCast;
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
                player.animator.SetTrigger("Dead");
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
            resetGameTime -= Time.deltaTime;
            if (resetGameTime <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
            }

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

    public void MapShrink() {
        for (int i = 0; i < 360; i += 22) {
            int randomY = Random.Range(30, 95);
            float ang = i; //Random.value * 360;
            Vector3 pos;
            pos.x = circleCenter.x + circleRadius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.z = circleCenter.z + circleRadius * Mathf.Cos(ang * Mathf.Deg2Rad);
            pos.y = circleCenter.y + randomY;
            Instantiate(giantFireball, pos, Quaternion.identity);
        }

        CurrentPlayer.enabled = false;
        FindObjectOfType<Canvas>().enabled = false;
        circleRadius -= 7f;
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

        FindObjectOfType<Canvas>().enabled = true;
        FindObjectOfType<DeathRainSpellCamera>().spellHitPointIndicator.enabled = false;
        if (CurrentPlayer) {
            CurrentPlayer?.Enable();

            for (int i = 0; i < CurrentPlayer.spells.Count; i++) {
                spellImages[i].sprite = CurrentPlayer.spells[i].spellImage;
                spellImages[i].color = CurrentPlayer.spells[i].spellImageColor;

            }
        }
        
    }

    #endregion

}

public struct PlayerInfo {

    public Player player;
    public PlayerUI playerUI;
    public bool dead;

}