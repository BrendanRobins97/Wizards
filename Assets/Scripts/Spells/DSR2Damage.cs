﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSR2Damage : MonoBehaviour
{
    public int contactDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Player>().Damage(contactDamage);
        }
    }
}
