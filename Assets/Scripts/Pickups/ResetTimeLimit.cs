using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTimeLimit : MonoBehaviour
{
    private Player player;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        this.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        if (player == null)
        {
            Debug.Log("No Player Found");
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (player != null)
        {
            if (col.tag == "Player")
            {
                gm.currentTurnTimeLeft = 20f;
                Destroy(this.gameObject, .02f);
            }
        }
    }
}
