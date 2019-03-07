using UnityEngine;
using System.Collections;

public class PlayAnimOnKeyUp : MonoBehaviour {

    public GameObject mainProjectile;
    public GameObject projectile1;
    public GameObject projectile2;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem mainParticleSystem;
    public Transform projectileAim;
    public GameObject Instance;
    public bool canfire;

    private void Start()
    {
        mainProjectile = projectile1;
        mainParticleSystem = particle1;
        canfire = true;
    }

    // Update is called once per frame
    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainProjectile = projectile1;
            mainParticleSystem = particle1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mainProjectile = projectile2;
            mainParticleSystem = particle2;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if(canfire == true)
            {
                Instance = Instantiate(mainProjectile);
                Instance.transform.SetParent(projectileAim);
                Instance.SetActive(true);
                Destroy(Instance, 3.5f);
                canfire = false;
                Invoke("firecooldown", .5f);
            }

        }

	}

    void firecooldown()
    {
        canfire = true;
    }
}
