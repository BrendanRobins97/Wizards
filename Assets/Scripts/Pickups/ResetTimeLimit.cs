using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResetTimeLimit : MonoBehaviour
{
    private Player player;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RawImage image;
    private GameManager gm;

    private bool pickedUp = false;

    private float displayTime;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        player = gm.GetComponent<GameManager>().CurrentPlayer;
        displayTime -= Time.deltaTime;
        if (pickedUp)
        {
            if (displayTime <= 0 && pickedUp)
            {
                //text.gameObject.SetActive(false);
                image.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }

            if (player == null)
            {
                Debug.Log("No Player Found");
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (player != null)
        {
            if (col.tag == "Player" && player == gm.GetComponent<GameManager>().CurrentPlayer)
            {
                gm.currentTurnTimeLeft = gm.turnTime;
                //text.gameObject.SetActive(true);
                //text.text = "Time Reset";
                image.gameObject.SetActive(true);
                pickedUp = true;
                displayTime = 2f;
                player.numUlt++;
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
