using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBoost : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        player = GameObject.FindObjectOfType<Player>();
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
                player.GetComponent<Player>().stamina = player.GetComponent<Player>().stamina + 20f;
                Destroy(this.gameObject, .02f);
            }
        }
    }
}
