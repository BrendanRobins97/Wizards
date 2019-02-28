// File: Spell.cs
// Author: Brendan Robinson
// Date Created: 02/19/2019
// Date Last Modified: 02/28/2019

using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell", order = 0)]
public class Spell : ScriptableObject {

    #region Fields

    public SpellObject spellPrefab;
    public float       speed;
    public int         contactDamage;
    public bool        affectedByCharge;
    public float       duration          = 5f;
    public float       collisionHoleSize = 2f;

    #endregion

    #region Methods

    public void ThrowSpell(Vector3 startPosition, Vector3 direction, float charge) {
        SpellObject spell = Instantiate(spellPrefab, startPosition, Quaternion.identity);
        if (!affectedByCharge) { charge = 1; }
        spell.GetComponent<Rigidbody>().velocity = direction * charge * speed;
        spell.transform.forward = direction;
        spell.damage = contactDamage;
        spell.collisionHoleSize = collisionHoleSize;
        spell.Disable(duration);
    }

    #endregion

}