using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResetTimeLimit : MonoBehaviour
{
    private Player player;
    [SerializeField] private TextMeshProUGUI text;
    private GameManager gm;

    private bool pickedup = false;

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
        if (displayTime <= 0 && pickedup)
        {
            text.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
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
                text.gameObject.SetActive(true);
                text.text = "Time Reset";
                pickedup = true;
                displayTime = 2f;
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
