// File: GameManager.cs
// Contributors: Brendan Robinson
// Date Created: 05/11/2019
// Date Last Modified: 05/12/2019

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
    [SerializeField]
    private Animator spellBarAnimator;
    [SerializeField]
    private GameObject pressEnterText;
    [SerializeField]
    private List<Image> spellImages;
    [SerializeField]
    private List<TextMeshProUGUI> spellDescriptions;
    [SerializeField]
    private TextMeshProUGUI ultCharge;
    [SerializeField]
    private GameObject pauseText;
    [SerializeField]
    private Canvas mainCanvas;

    [Header("Prefabs")]
    [SerializeField]
    private List<GameObject> playerPrefabs;
    [SerializeField]
    private GameObject giantFireball;

    [Header("Game Settings")]
    public float currentTurnTimeLeft;
    public float timeAfterSpellCast = 5f;
    public float gameStartTime = 5f;
    [SerializeField]
    private float spawnWidth = 40f;
    [SerializeField]
    public float turnTime = 20f;
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
    [SerializeField]
    private Transform[] cameras;

    private List<PlayerInfo> players = new List<PlayerInfo>();
    private bool endOfTurn, newRound, meteorShower;
    private bool gameStarted;
    private float circleRadius;
    private Vector3 circleCenter;
    private bool nextTurn;
    private bool gameOver;
    private bool paused;
    private float timeSincePaused;

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
        if (ps) { numPlayers = ps.numPlayers; }
        mapShrinkNumber = 0;
        circleCenter = new Vector3(TerrainManager.instance.width / 2.0f, 0, TerrainManager.instance.length / 2.0f);
        circleRadius = TerrainManager.instance.width / 2.0f - 5.0f;
        currentTurnTimeLeft = gameStartTime;
        for (int i = 0; i < numPlayers; i++) {
            PlayerInfo newPlayerInfo;
            float angle = i * 360f / numPlayers;
            Vector3 spawnLocation = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * spawnWidth + circleCenter.x,
                TerrainManager.instance.height, Mathf.Sin(Mathf.Deg2Rad * angle) * spawnWidth + circleCenter.z);
            Ray ray = new Ray(spawnLocation, Vector3.down);
            Physics.Raycast(ray, out RaycastHit rayHit);

            Player player;
            if (ps) {
                player = Instantiate(playerPrefabs[ps.playersPicked[i]], rayHit.point,
                        Quaternion.identity,
                        transform)
                    .GetComponent<Player>();
            }
            else {
                player = Instantiate(playerPrefabs[i], rayHit.point,
                        Quaternion.identity,
                        transform)
                    .GetComponent<Player>();
            }
            player.index = i;
            player.transform.LookAt(new Vector3(TerrainManager.instance.width / 2f, rayHit.point.y,
                TerrainManager.instance.length / 2f));
            PlayerUI playerUI = Instantiate(playerUIPrefab, playerInfoContainer).GetComponent<PlayerUI>();
            playerUI.playerImage.sprite = player.icon;
            playerUI.playerImage.color = player.color;
            playerUI.playerName.text = player.wizardName;
            playerUI.EndTurn();

            newPlayerInfo.player = player;
            newPlayerInfo.playerUI = playerUI;
            newPlayerInfo.dead = false;
            players.Add(newPlayerInfo);
        }
        // Start these UI components turned off
        gameOverText.gameObject.SetActive(false);
        pressEnterText.SetActive(false);
        pauseText.SetActive(false);

        roundNumber = 0;
        playerTurn = 0;
        numPlayersLeft = numPlayers;
        if (ps) { Destroy(ps.gameObject); }
        for (int i = 0; i < CurrentPlayer.spells.Count; i++) {
            spellImages[i].sprite = CurrentPlayer.spells[i].spellImage;
            spellImages[i].color = CurrentPlayer.spells[i].spellImageColor;
            spellDescriptions[i].text = CurrentPlayer.spells[i].description;
        }
        // Register camera containers for screen shake
        foreach (Transform cam in cameras) { CameraController.instance.RegisterCamera(cam); }
        gameOver = false;
    }

    private void Update() {
        if (!nextTurn && (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start")) &&
            SceneManager.GetActiveScene().name == "MainTestScene") { Pause(); }

        if (playerTurn == 0 && newRound && roundNumber / numPlayers > mapShrinkNumber) {
            if (meteorShower == false) {
                CurrentPlayer.playerCamera.enabled = false;
                nextTurn = false;
                currentTurnTimeLeft = turnTime + 5f;
                meteorShower = true;
                endOfTurn = true;
                mainCamera.enabled = true;
                MapShrink();
                pressEnterText.SetActive(false);

                return;
            }

            if (currentTurnTimeLeft <= turnTime) {
                CurrentPlayer.EnableCamera();
                mainCanvas.enabled = true;
                mapShrinkNumber += 2;

                mainCamera.enabled = false;
                meteorShower = false;
                newRound = false;
                endOfTurn = false;
                nextTurn = true;
                pressEnterText.SetActive(true);
            }
        }

        if (nextTurn && !gameOver) {
            // Do nothing until player presses start
            if (Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Return)) {
                nextTurn = false;
                spellBarAnimator.SetTrigger("HideInfo");
                if (CurrentPlayer) {
                    CurrentPlayer.playerCamera.enabled = true;
                    CurrentPlayer?.Enable();
                }
                currentTurnTimeLeft = turnTime;
                pressEnterText.SetActive(false);
            }
            turnText.text = "Time Left: " + (int) (currentTurnTimeLeft + 0.98f);

            return;
        }
        //Buttons to press to quit when in a build
        if (Input.GetButtonDown("Start") && Input.GetButtonDown("Fire1")) { ExitGame(); }
        // Update the current time left and update the timer text
        currentTurnTimeLeft -= Time.deltaTime;
        currentTurnTimeLeft = Mathf.Clamp(currentTurnTimeLeft, -0.9f, Mathf.Infinity);
        turnText.text = "Time Left: " + (int) (currentTurnTimeLeft + 0.98f);

        if (playerTurn > 0) { newRound = true; }
        if (mainCamera.enabled == false) { FindObjectOfType<Canvas>().enabled = true; }
        if (!gameStarted) { // Handle game start behavior
            mainCanvas.enabled = false;
            // Start game when initial timer hits 0
            if (currentTurnTimeLeft < 0) {
                CurrentPlayer.playerCamera.enabled = true;
                StartTurn();
                mainCamera.enabled = false;
                gameStarted = true;
                mainCanvas.enabled = true;
                return;
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
            foreach (Transform child in mainCanvas.transform) { child.gameObject.SetActive(false); }
            gameOverText.gameObject.SetActive(true);
            resetGameTime -= Time.deltaTime;
            if (resetGameTime <= 0) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2); }
            gameOver = true;
        }
    }

    private void StartTurn() {
        endOfTurn = false;
        currentTurnTimeLeft = turnTime;
        nextTurn = true;
        for (int i = 0; i < players.Count; i++) {
            if (players[i].player) {
                players[i].player.nameUI.enabled = true;
                players[i].player.healthBar.SetActive(true);
            }
        }
        CurrentPlayer.nameUI.enabled = false;
        CurrentPlayer.healthBar.SetActive(false);

        if (meteorShower == false) {
            spellBarAnimator.SetTrigger("ShowInfo");
            players[playerTurn].playerUI.StartTurn();
            ResetSpellImages();
        }
        pressEnterText.SetActive(true);
    }

    public void UpdateSpellImage(int index) {
        for (int i = 0; i < spellImages.Count; i++) {
            spellImages[i].color =
                new Color(spellImages[i].color.r, spellImages[i].color.g, spellImages[i].color.b, 0.1f);
        }
        spellImages[index].color = new Color(spellImages[index].color.r, spellImages[index].color.g,
            spellImages[index].color.b, 1);
        if (CurrentPlayer.numUlt <= 0) {
            spellImages[3].color =
                new Color(spellImages[3].color.r, spellImages[3].color.g, spellImages[3].color.b, 0.5f);
        }
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
        mainCanvas.enabled = false;
        circleRadius -= 7f;
    }

    public void Damage(int damage, int index) { players[index].playerUI.Damage(damage); }

    private void NextPlayerTurn() {
        // Disable current player
        CurrentPlayer.Disable();
        players[playerTurn].playerUI.EndTurn();
        roundNumber++;
        // Find the next available player
        int count = 0;
        do {
            playerTurn = (playerTurn + 1) % numPlayers;
            count++;
        }
        while (CurrentPlayer == null && count <= numPlayers);

        for (int i = 0; i < players.Count; i++) {
            if (players[i].player) {
                players[i].player.nameUI.enabled = true;
                players[i].player.healthBar.SetActive(true);
            }
            
        }
        CurrentPlayer.nameUI.enabled = false;
        CurrentPlayer.healthBar.SetActive(false);

        mainCanvas.enabled = true;
        FindObjectOfType<DeathRainSpellCamera>().spellHitPointIndicator.enabled = false;

        // Bring up press start to start turn overlay
        pressEnterText.SetActive(true);
        nextTurn = true;
        spellBarAnimator.SetTrigger("ShowInfo");
        ResetSpellImages();
        CurrentPlayer.EnableCamera();
    }

    private void ExitGame() //Exit game when in build
    {
        Application.Quit();
    }

    private void Pause() {
        if (!paused) {
            paused = true;
            Time.timeScale = 0;
            timeSincePaused = Time.realtimeSinceStartup;
            pauseText.SetActive(true);
        }
        else if (paused && Time.realtimeSinceStartup - timeSincePaused > .5f) {
            paused = false;
            Time.timeScale = 1;
            timeSincePaused = 0;
            pauseText.SetActive(false);
        }
    }

    private void ResetSpellImages() {
        for (int i = 0; i < spellImages.Count; i++) {
            spellImages[i].color =
                new Color(spellImages[i].color.r, spellImages[i].color.g, spellImages[i].color.b, 1f);
        }
        for (int i = 0; i < CurrentPlayer.spells.Count; i++) {
            spellImages[i].sprite = CurrentPlayer.spells[i].spellImage;
            spellImages[i].color = CurrentPlayer.spells[i].spellImageColor;
            spellDescriptions[i].text = CurrentPlayer.spells[i].description;
        }
        ultCharge.text = "x" + CurrentPlayer.numUlt;
        if (CurrentPlayer.numUlt <= 0) {
            spellImages[3].color =
                new Color(spellImages[3].color.r, spellImages[3].color.g, spellImages[3].color.b, 0.5f);
        }
    }

    #endregion

}

public struct PlayerInfo {

    public Player player;
    public PlayerUI playerUI;
    public bool dead;

}