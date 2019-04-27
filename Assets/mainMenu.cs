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
    float timer2;
    private bool startGame = true;
    public static mainMenu instance = null;
    public AudioSource audioData;
    public AudioClip mainTheme;
    int hoverNumPlayers;
    public GameObject arrow;
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
        hoverNumPlayers = 4;
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
        if(state == 3 || state == 5)
        {
            if (hoverNumPlayers == 1) { arrow.transform.position = new Vector3(-3.01f, 1.37f, 4.594f); }
            if (hoverNumPlayers == 2) { arrow.transform.position = new Vector3(-3.429f, 1.38f, 4.48f); }
            if (hoverNumPlayers == 3) { arrow.transform.position = new Vector3(-3.925f, 1.392f, 4.344f); }
            if (hoverNumPlayers == 4) { arrow.transform.position = new Vector3(-4.48f, 1.405f, 4.193f); }
            if (hoverNumPlayers == 5) { arrow.transform.position = new Vector3(-5.178f, 1.421f, 4.003f); }
            if (hoverNumPlayers == 6) { arrow.transform.position = new Vector3(-5.696f, 1.433f, 3.862f); }
            if (hoverNumPlayers == 7) { arrow.transform.position = new Vector3(-6.34f, 1.448f, 3.686f); }
            if (hoverNumPlayers == 8) { arrow.transform.position = new Vector3(-6.881f, 1.461f, 3.539f); }
        }
        if (state == 4)
        {
            timer += Time.deltaTime;
        }
        if (state == 5)
        {
            timer2 += Time.deltaTime;
            if(timer2 >= .25)
            {
                state = 3;
                timer2 = 0.0f;
            }
        }
        if(timer >= 2.6f && startGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            startGame = false;
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (state == 1 && Input.GetAxisRaw("Submit") > 0 || Input.GetButtonUp("Start"))
            {
                myAnimator.Play("part3");
                state = 3;
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
            else if (state == 3 && Input.GetAxisRaw("Submit") > 0.0)
            {
                numPlayers = hoverNumPlayers;
                myAnimator.Play("part4");
                state = 4;
            }
            else if (state == 3)
            {
                if (Input.GetAxis("Horizontal") < -0.2)
                {
                    if (hoverNumPlayers > 2)
                    {
                        hoverNumPlayers -= 1;
                        print(timer2 % .5);
                        state = 5;
                    }
                }
                else if (Input.GetAxis("Horizontal") > 0.2)
                {
                    if (hoverNumPlayers < 8)
                    {
                        hoverNumPlayers += 1;
                        print("g");
                        state = 5;
                    }
                }
            }
        }
    }
}
