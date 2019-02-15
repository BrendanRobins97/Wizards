// File: SpellObject.cs
// Author: Brendan Robinson
// Date Created: 02/15/2019
// Date Last Modified: 02/15/2019
// Description: 

using UnityEngine;

public class SpellObject : MonoBehaviour
{
    [HideInInspector] public int damage = 10;

    private MeshCollider collider;

    // Start is called before the first frame update
    private void Start()
    {
        collider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damage(damage);
        }
    }
}