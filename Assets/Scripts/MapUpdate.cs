using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUpdate : MonoBehaviour
{
    public Camera updateCamera;
    [SerializeField] private Animator anim;
    private Player player, startingPlayer;
    private bool reset = false;
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
        if (GameManager.instance.GetComponent<GameManager>().playerTurn == 0 && resetTime > 0)
        {
            UpdateMap();
            reset = true;
        }
        else
        {
            updateCamera.enabled = false;
            anim.SetBool("isUpdate", false);
            
        }

    }

    public void UpdateMap()
    {
        resetTime = 2f;
        StartCoroutine(CameraUp());
        reset = false;
    }
    IEnumerator CameraUp()
    {
        resetTime -= Time.deltaTime;
        updateCamera.enabled = true;
        anim.SetBool("isUpdate", true);
        GameManager.instance.GetComponent<GameManager>().currentTurnTimeLeft = 20;
        yield return new WaitForSeconds(1.0f);
        
    }
}
