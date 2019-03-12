using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUpdate : MonoBehaviour
{
    public Camera updateCamera;
    [SerializeField] private Animator anim;
    private Player player, startingPlayer;
    private bool reset = true;
    private float resetTime = 2f;
    private int numPlayers;
    // Start is called before the first frame update
    void Start()
    {
        updateCamera.enabled = false;
        anim = FindObjectOfType<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetComponent<GameManager>().playerTurn == 0 && reset)
        {
            //UpdateMap();
            Debug.Log("MapUpdating." + " reset is false");
            reset = false;
        }
        else
        {
            updateCamera.enabled = false;
            anim.SetBool("isUpdate", false);
            
        }

        if (GameManager.instance.GetComponent<GameManager>().playerTurn == GameManager.instance.GetComponent<GameManager>().numPlayersLeft-1)
        {
            reset = true;
            Debug.Log("reset = true");
        }
        
    }

    public void UpdateMap()
    {
        resetTime = 5f;
        StartCoroutine(CameraUp());
        reset = false;
    }
    IEnumerator CameraUp()
    {
            resetTime -= Time.deltaTime;
            //updateCamera.enabled = true;
            //anim.SetBool("isUpdate", true);
            GameManager.instance.GetComponent<GameManager>().currentTurnTimeLeft = 5;
            yield return new WaitForSeconds(5.0f);

    }
}
