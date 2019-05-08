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
    public int stage1Index = 0;
    public List<Player> players;
    public List<Player> playerBases;
    public TextMeshProUGUI text;
    public int numPlayers = 3;
    private bool picked = false;
    private int playerPicking = 0;
    private int baseIndex;
    private float gameStartTimer = -4;
    private bool show = true;
    private bool showTutorial = false;
    private bool showPlayerPicked = false;
    private bool stage1, stage2 = false;
    private float showPickedTime = 2f;
    private float axisCooldownTime = .33f;
    public List<int> playersPicked = new List<int>();
    private List<int> used = new List<int>();
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

        stage1 = true;
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
        axisCooldownTime -= Time.deltaTime;
        if (showPlayerPicked)
        {
            text.text = "Player Already Chosen. Choose Another Player.";
            showPickedTime -= Time.deltaTime;
            if (showPickedTime <= 0)
            {
                showPlayerPicked = false;
                showPickedTime = 3f;
            }
        }

        if (showTutorial)
        {
            canvas.enabled = false;
            tutorialCanvas.enabled = true;
        }

        if (gameStartTimer > 3 && gameStartTimer < 6 && !showTutorial)
        {
            text.text = "Player " + (playerDisplay - 1) + " You Chose " + players[currentIndex].name;
            //used.Add(currentIndex);
        }

        if (gameStartTimer <= 3 && gameStartTimer > 0 && !showTutorial)
        {
            text.text = "Lets Play!";
            gameStartTimer = 15f;
            showTutorial = true;
        }

        if (gameStartTimer <= 0 && gameStartTimer >= -2 || Input.GetButtonUp("Start") || Input.GetKeyDown(KeyCode.Return))
        {
            Input.ResetInputAxes();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (stage1)
        {
            Vector3 camPos = new Vector3(playerBases[stage1Index].transform.position.x,
                playerBases[stage1Index].transform.position.y + 2, playerBases[stage1Index].transform.position.z + 5);
            //camera.transform.position = camPos;
            camera.transform.position = Vector3.Lerp(camera.transform.position, camPos, Time.deltaTime * 2f);
            camera.transform.LookAt(playerBases[stage1Index].transform.position);
            for (int i = 0; i < players.Count; i++)
            {
                players[i].gameObject.SetActive(false);
                players[i].animator.SetTrigger("Idle");
                players[i].GetComponent<CapsuleCollider>().enabled = false;
            }

            if (show && !showPlayerPicked)
            {
                text.text = "Player " + playerDisplay + ": " +
                            " Press A/Left Click To Select a Character.";
            }

            //if (Input.GetButtonDown("Jump")&& playerPicking < numPlayers)
            if (Input.GetAxis("Horizontal") > .2f && playerPicking < numPlayers && axisCooldownTime < 0)
            {
                show = true;
                axisCooldownTime = .33f;
                playerBases[stage1Index].AnimTriggerReset();
                stage1Index++;
                for (int i = 0; i < used.Count; i++)
                {
                    if (currentIndex == used[i])
                    {
                        //currentIndex++;
                    }
                }

                if (stage1Index >= playerBases.Count)
                {
                    stage1Index = 0;
                }
            }

            if (Input.GetAxis("Horizontal") < -.2f && playerPicking < numPlayers && axisCooldownTime < 0)
            {
                show = true;
                axisCooldownTime = .3f;
                playerBases[stage1Index].AnimTriggerReset();
                stage1Index--;
                for (int i = 0; i < used.Count; i++)
                {
                    if (stage1Index == used[i])
                    {
                        //currentIndex++;
                    }
                }

                if (stage1Index < 0)
                {
                    stage1Index = playerBases.Count - 1;
                }
            }

            if (Input.GetButtonUp("Fire1") && playerPicking < numPlayers)
            {
                axisCooldownTime = .3f;
                stage1 = false;
                stage2 = true;
                if (stage1Index == 0)
                    currentIndex = stage1Index;
                if (stage1Index == 1)
                    currentIndex = 5;
                if (stage1Index == 2)
                    currentIndex = 9;
                if (stage1Index == 3)
                    currentIndex = 13;
            }
        }

        if (stage2)
        {
            if (Input.GetButtonUp("B")||Input.GetKeyUp(KeyCode.B))
            {
                stage1 = true;
                stage2 = false;
            }
            Debug.Log("Looking at " + players[currentIndex].name + " at index " + currentIndex);
            baseIndex = stage1Index;
            Vector3 camPos = new Vector3(players[currentIndex].transform.position.x,
                players[currentIndex].transform.position.y + 2, players[currentIndex].transform.position.z + 5);
            //camera.transform.position = camPos;
            camera.transform.position = Vector3.Lerp(camera.transform.position, camPos, Time.deltaTime * 2f);
            camera.transform.LookAt(players[currentIndex].transform.position);
            text.text = "Player " + playerDisplay + ": " + "Choose your skin.";
            if (baseIndex == 0)
            {
                for (int i = 0; i <= 4; i++)
                {
                    if (i == currentIndex)
                    {
                        players[currentIndex].animator.SetFloat("Forward Amount", 0);
                        players[currentIndex].gameObject.SetActive(true);
                        players[currentIndex].AnimTriggerReset();
                        players[currentIndex].animator.SetTrigger("Idle");
                    }
                    else
                    {
                        players[i].animator.SetFloat("Forward Amount", 0);
                        players[i].gameObject.SetActive(false);
                        players[i].AnimTriggerReset();
                        players[i].animator.SetTrigger("Idle");
                    }
                }

                if (Input.GetButtonUp("Fire1") && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    stage1 = true;
                    stage2 = false;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            picked = true;
                        }
                    }

                    if (!picked)
                    {
                        text.text = "Player " + playerDisplay + " You Chose " + players[currentIndex].name +
                                    "\nPlayer " + (playerDisplay + 1) + " Choose Your Character.";
                        show = false;
                        playersPicked.Add(currentIndex);
                        used.Add(currentIndex);
                        playerPicking++;
                        //players[currentIndex].gameObject.SetActive(false);
                        players[currentIndex].animator.SetTrigger("Hit");
                        //currentIndex++;
                        showPlayerPicked = false;
                        if (playerPicking >= numPlayers)
                        {
                            gameStartTimer = 6f;
                        }
                    }
                    else
                    {
                        showPlayerPicked = true;
                        picked = false;
                    }
                }

                if (Input.GetAxis("Horizontal") > .2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .3f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex++;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex > 4)
                    {
                        currentIndex = 0;
                    }
                }

                if (Input.GetAxis("Horizontal") < -.2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .3f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex--;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex < 0)
                    {
                        currentIndex = 4;
                    }
                }
            }

            if (baseIndex == 1)
            {
                for (int i = 5; i <= 8; i++)
                {
                    if (i == currentIndex)
                    {
                        players[currentIndex].animator.SetFloat("Forward Amount", 0);
                        players[currentIndex].gameObject.SetActive(true);
                        //players[currentIndex].AnimTriggerReset();
                        players[currentIndex].animator.SetTrigger("Idle");
                    }
                    else
                    {
                        players[i].animator.SetFloat("Forward Amount", 0);
                        players[i].gameObject.SetActive(false);
                        //players[i].AnimTriggerReset();
                        players[i].animator.SetTrigger("Idle");
                    }
                }

                if (Input.GetButtonUp("Fire1") && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    stage1 = true;
                    stage2 = false;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            picked = true;
                        }
                    }

                    if (!picked)
                    {
                        text.text = "Player " + playerDisplay + " You Chose " + players[currentIndex].name +
                                    "\nPlayer " + (playerDisplay + 1) + " Choose Your Character.";

                        show = false;
                        playersPicked.Add(currentIndex);
                        used.Add(currentIndex);
                        playerPicking++;
                        //players[currentIndex].gameObject.SetActive(false);
                        players[currentIndex].animator.SetTrigger("Hit");
                        //currentIndex++;
                        showPlayerPicked = false;
                        if (playerPicking >= numPlayers)
                        {
                            gameStartTimer = 6f;
                        }
                    }
                    else
                    {
                        showPlayerPicked = true;
                        picked = false;
                    }
                }

                if (Input.GetAxis("Horizontal") > .2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .33f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex++;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex > 8)
                    {
                        currentIndex = 5;
                    }
                }

                if (Input.GetAxis("Horizontal") < -.2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .33f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex--;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex < 5)
                    {
                        currentIndex = 8;
                    }
                }
            }

            if (baseIndex == 2)
            {
                for (int i = 9; i <= 12; i++)
                {
                    if (i == currentIndex)
                    {
                        players[currentIndex].animator.SetFloat("Forward Amount", 0);
                        players[currentIndex].gameObject.SetActive(true);
                       // players[currentIndex].AnimTriggerReset();
                        players[currentIndex].animator.SetTrigger("Idle");
                    }
                    else
                    {
                        players[i].animator.SetFloat("Forward Amount", 0);
                        players[i].gameObject.SetActive(false);
                        //players[i].AnimTriggerReset();
                        players[i].animator.SetTrigger("Idle");
                    }
                }

                if (Input.GetButtonUp("Fire1") && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    stage1 = true;
                    stage2 = false;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            picked = true;
                        }
                    }

                    if (!picked)
                    {
                        text.text = "Player " + playerDisplay + " You Chose " + players[currentIndex].name +
                                    "\nPlayer " + (playerDisplay + 1) + " Choose Your Character.";

                        show = false;
                        playersPicked.Add(currentIndex);
                        used.Add(currentIndex);
                        playerPicking++;
                        //players[currentIndex].gameObject.SetActive(false);
                        players[currentIndex].animator.SetTrigger("Hit");
                        //currentIndex++;
                        showPlayerPicked = false;
                        if (playerPicking >= numPlayers)
                        {
                            gameStartTimer = 6f;
                        }
                    }
                    else
                    {
                        showPlayerPicked = true;
                        picked = false;
                    }
                }

                if (Input.GetAxis("Horizontal") > .2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .33f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex++;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex > 12)
                    {
                        currentIndex = 9;
                    }
                }

                if (Input.GetAxis("Horizontal") < -.2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .33f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex--;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex < 9)
                    {
                        currentIndex = 12;
                    }
                }
            }

            if (baseIndex == 3)
            {
                for (int i = 13; i <= 16; i++)
                {
                    if (i == currentIndex)
                    {
                        players[currentIndex].animator.SetFloat("Forward Amount", 0);
                        players[currentIndex].gameObject.SetActive(true);
                        //players[currentIndex].AnimTriggerReset();
                        players[currentIndex].animator.SetTrigger("Idle");
                    }
                    else
                    {
                        players[i].animator.SetFloat("Forward Amount", 0);
                        players[i].gameObject.SetActive(false);
                        //players[i].AnimTriggerReset();
                        players[i].animator.SetTrigger("Idle");
                    }
                }

                if (Input.GetButtonUp("Fire1") && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    stage1 = true;
                    stage2 = false;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            picked = true;
                        }
                    }

                    if (!picked)
                    {
                        text.text = "Player " + playerDisplay + " You Chose " + players[currentIndex].name +
                                    "\nPlayer " + (playerDisplay + 1) + " Choose Your Character.";

                        show = false;
                        playersPicked.Add(currentIndex);
                        used.Add(currentIndex);
                        playerPicking++;
                        //players[currentIndex].gameObject.SetActive(false);
                        players[currentIndex].animator.SetTrigger("Hit");
                        //currentIndex++;
                        showPlayerPicked = false;
                        if (playerPicking >= numPlayers)
                        {
                            gameStartTimer = 6f;
                        }
                    }
                    else
                    {
                        showPlayerPicked = true;
                        picked = false;
                    }
                }

                if (Input.GetAxis("Horizontal") > .2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .33f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex++;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex > 16)
                    {
                        currentIndex = 13;
                    }
                }

                if (Input.GetAxis("Horizontal") < -.2f && playerPicking < numPlayers && axisCooldownTime < 0)
                {
                    show = true;
                    axisCooldownTime = .33f;
                    players[currentIndex].AnimTriggerReset();
                    currentIndex--;
                    for (int i = 0; i < used.Count; i++)
                    {
                        if (currentIndex == used[i])
                        {
                            //currentIndex++;
                        }
                    }

                    if (currentIndex < 13)
                    {
                        currentIndex = 16;
                    }
                }
            }
        }
    }
}
