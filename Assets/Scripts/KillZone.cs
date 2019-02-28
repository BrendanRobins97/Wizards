// File: KillZone.cs
// Author: Brendan Robinson
// Date Created: 02/19/2019
// Date Last Modified: 02/28/2019

using UnityEngine;

public class KillZone : MonoBehaviour {

    #region Methods

    private void OnTriggerEnter(Collider collision) {
        // Kill player if they touch this
        if (collision.gameObject.CompareTag("Player")) { collision.gameObject.GetComponentInParent<Player>().Kill(); }
    }

    #endregion

}