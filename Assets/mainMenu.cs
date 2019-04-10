using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class mainMenu : MonoBehaviour
{
    GameObject myCamera;
    Animator myAnimator;
    int state;
    bool teams;
    public static int numPlayers;
    float timer;
    private bool startGame = true;
    public static mainMenu instance = null;
    public AudioSource audioData;
    public AudioClip mainTheme;
    // Start is called before the first frame update
    void Start()
    {
        audioData.clip = mainTheme;
        audioData.Play();
        state = 1;
        myCamera = GameObject.Find("Main Camera");
        myAnimator = myCamera.GetComponent<Animator>();
        //myAnimator.Play("p1");
        myAnimator.Play("part1");
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

    public int NumPlayers()
    {
        return numPlayers;
    }
    // Update is called once per frame
    void Update()
    {
        if (state == 4)
        {
            timer += Time.deltaTime;
        }
        if(timer >= 2.6f && startGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            startGame = false;
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (state == 1 && Input.GetAxisRaw("Submit") > 0)
            {
                myAnimator.Play("part2");
                state = 2;
            }
            else if (state == 2 && Input.GetAxisRaw("Cancel") > 0)
            {
                teams = false;
                myAnimator.Play("part3");
                state = 3;
            }
            else if (state == 2 && Input.GetAxisRaw("Submit") > 0)
            {
                teams = true;
                myAnimator.Play("part3");
                state = 3;
            }
            else if (state == 3 && Input.GetKeyDown("1"))
            {
                numPlayers = 1;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("2"))
            {
                numPlayers = 2;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("3"))
            {
                numPlayers = 3;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("4"))
            {
                numPlayers = 4;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("5"))
            {
                numPlayers = 5;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("6"))
            {
                numPlayers = 6;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("7"))
            {
                numPlayers = 7;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("8"))
            {
                numPlayers = 8;
                myAnimator.Play("part4");
                state = 4;
            }
        }
    }
}
