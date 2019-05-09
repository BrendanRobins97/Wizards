// File: FloatingTextManager.cs
// Contributors: Brendan Robinson
// Date Created: 04/03/2019
// Date Last Modified: 04/03/2019

using TMPro;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour {

    #region Constants

    private const float               textDuration = 2f;
    public static FloatingTextManager instance;

    #endregion

    #region Fields

    public GameObject floatingTextPrefab;

    #endregion

    #region Methods

    private void Start() {
        if (instance != null) { Destroy(this); }
        else { instance = this; }
    }

    public void SpawnDamageText(Vector3 position, int damage) {
        if (damage != 0)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            floatingText.GetComponentOnObject<TextMeshProUGUI>().text = damage.ToString();
            Destroy(floatingText, textDuration);
        }
    }

    #endregion

}