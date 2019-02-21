// File: SpellObject.cs
// Author: Brendan Robinson
// Date Created: 02/15/2019
// Date Last Modified: 02/15/2019
// Description: 

using System.Collections.Generic;
using UnityEngine;

public class SpellObject : MonoBehaviour
{
    [HideInInspector] public int damage = 10;
    [HideInInspector] public float destroyTime = 10;

    private List<Player> playersHit = new List<Player>();

    [SerializeField] private List<Component> componentsToDestroy;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider collision)
    {
        DestroyComponents();
        Destroy(gameObject, 5);
        if (collision.gameObject.CompareTag("Player"))
        {

            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (playersHit.Contains(player))
            {
                return;
            }
            player.Damage(damage);
            playersHit.Add(player);
        }
    }

    public void Disable(float time) {
        Invoke("DestroyComponents", time);
    }

    private void DestroyComponents() {
        for (int i = 0; i < componentsToDestroy.Count; i++) {
            Destroy(componentsToDestroy[i]);
        }
    }
}