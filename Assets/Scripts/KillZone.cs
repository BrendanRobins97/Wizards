// File: KillZone.cs
// Author: Brendan Robinson
// Date Created: 02/15/2019
// Date Last Modified: 02/15/2019
// Description: Component to kill player on trigger enter

using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {   
        // Kill player if they touch this
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<Player>().Kill();
        }
    }
}