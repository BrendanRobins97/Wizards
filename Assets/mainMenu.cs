using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    GameObject myCamera;
    Animator myAnimator;
    int state;
    bool teams;
    int numPlayers;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        state = 1;
        myCamera = GameObject.Find("Main Camera");
        myAnimator = myCamera.GetComponent<Animator>();
        //myCamera.GetComponent<Animation>().Play("p1");
        myAnimator.Play("part1");
    }
    // Update is called once per frame
    void Update()
    {
        if (state == 4)
        {
            timer += Time.deltaTime;
        }
        if(timer >= 6.0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("2"))
            {
                numPlayers = 2;
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("3"))
            {
                numPlayers = 3;
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("4"))
            {
                numPlayers = 4;
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("5"))
            {
                numPlayers = 5;
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("6"))
            {
                numPlayers = 6;
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("7"))
            {
                numPlayers = 7;
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
            else if (state == 3 && Input.GetKeyDown("8"))
            {
                numPlayers = 8;
                myCamera.GetComponent<Animation>().Play("p4");
                state = 4;
            }
        }
    }
}
