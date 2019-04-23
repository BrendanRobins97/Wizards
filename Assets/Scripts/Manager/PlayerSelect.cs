using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    public Canvas canvas, tutorialCanvas;
    public static PlayerSelect instance;
    public Camera camera;
    public int currentIndex = 0;
    public List<Player> players;
    public TextMeshProUGUI text;
    public int numPlayers = 3;
    private int playerPicking = 0;
    private float gameStartTimer = -4;
    private bool show = true;
    private bool showTutorial = false;
    public List<int> playersPicked = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        Vector3 camPos = new Vector3(players[0].transform.position.x, players[0].transform.position.y+2, players[0].transform.position.z+5);
        camera.transform.position = camPos;
        camera.transform.LookAt(players[0].transform.position);
        mainMenu mm = FindObjectOfType<mainMenu>();
        if (mm)
        {
            numPlayers = mm.NumPlayers();
        }

        tutorialCanvas.enabled = false;
    }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        int playerDisplay = playerPicking + 1;
        gameStartTimer -= Time.deltaTime;
        if (showTutorial)
        {
            canvas.enabled = false;
            tutorialCanvas.enabled = true;
        }
        if (gameStartTimer > 3 && gameStartTimer < 6 && !showTutorial)
        {
            text.text = "Player " + (playerDisplay-1) + " You Chose " + players[currentIndex].name;
        }
        if(gameStartTimer <= 3 && gameStartTimer > 0 && !showTutorial)
        {
            text.text = "Lets Play!";
            gameStartTimer = 15f;
            showTutorial = true;
        }
        if (gameStartTimer <= 0 && gameStartTimer >= -2 || Input.GetButtonUp("Start")||Input.GetKeyDown(KeyCode.S))
        {
            Input.ResetInputAxes();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        Vector3 camPos = new Vector3(players[currentIndex].transform.position.x, players[currentIndex].transform.position.y+2, players[currentIndex].transform.position.z + 5);
        camera.transform.position = camPos;
        camera.transform.LookAt(players[currentIndex].transform.position);
        if (show)
        {
            text.text = "Player " + playerDisplay + ": " +
                        " Press Y/Space To Cycle " +
                        " Press A/Left Click To Select";
        }

        if (Input.GetButtonDown("Jump")&& playerPicking < numPlayers)
        {
            show = true;
            players[currentIndex].AnimTriggerReset();
            currentIndex++;
            if (currentIndex >= players.Count)
            {
                currentIndex = 0;
            }
        }

        if (Input.GetButtonUp("Fire1")&&playerPicking < numPlayers)
        {
            text.text = "Player " + playerDisplay + " You Chose " + players[currentIndex].name;
            show = false;
            playersPicked.Add(currentIndex);
            playerPicking++;
            players[currentIndex].animator.SetTrigger("Hit");
           if (playerPicking >= numPlayers)
            {
                
                gameStartTimer = 6f;
            }
        }
    }
}
