// File: PlayerUI.cs
// Contributors: Brendan Robinson
// Date Created: 04/26/2019
// Date Last Modified: 05/07/2019

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    #region Fields

    public Slider healthBar;
    public Slider staminaBar;
    public Image playerImage;
    public TextMeshProUGUI playerName;
    public Image background;
    public Animator playerNameAnimator;
    public TextMeshProUGUI damageText;
    public Animator damageTextAnimator;

    #endregion

    #region Methods

    public void StartTurn() {
        background.enabled = true;
        playerNameAnimator.SetBool("NamePop", true);
    }

    public void EndTurn() {
        background.enabled = false;
        playerNameAnimator.SetBool("NamePop", false);
    }

    public void Damage(int damage) {
        if (damage != 0)
        {
            damageTextAnimator.SetTrigger("Reset");
            damageText.text = "-" + damage;
            damageTextAnimator.SetTrigger("Damage");
        }
    }

    #endregion

}