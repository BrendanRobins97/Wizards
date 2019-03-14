using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Spell
{
    public override void ThrowSpell(Vector3 direction, float charge) {
        if (!affectedByCharge) { charge = 1; }
        transform.forward = direction;
        Disable(duration);
    }
}
