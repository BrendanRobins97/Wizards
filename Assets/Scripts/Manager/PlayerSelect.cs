using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    public static PlayerSelect instance;
    public Camera camera;
    public int currentIndex = 0;
    public List<Player> players;

    public int numPlayers = 3;
    private int playerPicking = 0;
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
        Vector3 camPos = new Vector3(players[currentIndex].transform.position.x, players[currentIndex].transform.position.y+2, players[currentIndex].transform.position.z + 5);
        camera.transform.position = camPos;
        camera.transform.LookAt(players[currentIndex].transform.position);
        if (Input.GetButtonDown("Jump"))
        {
            currentIndex++;
            if (currentIndex >= players.Count)
            {
                currentIndex = 0;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            Debug.Log(playerPicking + " " + currentIndex + " " + numPlayers);
           playersPicked.Add(currentIndex);
            playerPicking++;
            if (playerPicking >= numPlayers)
            {
                Debug.Log("All Players have chosen");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
