using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell", order = 0)]
public class Spell : ScriptableObject
{
    public SpellObject spellPrefab;
    public float speed;
    public int contactDamage;

    public void ThrowSpell(Vector3 startPosition, Vector3 direction, float charge)
    {
        SpellObject spell = Instantiate(spellPrefab, startPosition, Quaternion.identity);
        spell.GetComponent<Rigidbody>().velocity = direction * charge * speed;
        spell.damage = contactDamage;
    }
}
