// File: HealthBar.cs
// Contributors: Brendan Robinson
// Date Created: 05/12/2019
// Date Last Modified: 05/12/2019

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    #region Fields

    [SerializeField]
    private Player player;

    private Slider healthBar;

    #endregion

    #region Methods

    private void Start() { healthBar = GetComponent<Slider>(); }

    private void Update() { healthBar.value = player.HealthPercent(); }

    #endregion

}