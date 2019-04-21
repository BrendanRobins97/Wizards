using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private float timer = 0f;

    private int numPlayers;
    // Start is called before the first frame update
    void Start()
    {
        Input.ResetInputAxes();
        mainMenu mm = FindObjectOfType<mainMenu>();
        if (mm)
        {
            numPlayers = mm.NumPlayers();
            Destroy(mm.gameObject);
        }

        PlayerSelect ps = FindObjectOfType<PlayerSelect>();
        if (ps)
        {
            numPlayers = ps.numPlayers;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
