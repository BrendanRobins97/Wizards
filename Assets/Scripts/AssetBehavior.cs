using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBehavior : MonoBehaviour
{
    [SerializeField] private float maxGroundAngle = 20;
    private float angle;

    private RaycastHit hitInfo;
    // Start is called before the first frame update
    void Start()
    {
        Physics.Raycast(this.transform.position, -Vector3.up, out hitInfo);
        angle = Vector3.Angle(hitInfo.normal, this.transform.forward);
        Debug.Log("Angle " + angle + " hit info normal " + hitInfo.normal);
        if (angle >= maxGroundAngle)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "FireSpell" || col.tag == "Spell")
        {
            Debug.Log("unfreezing");
            this.gameObject.GetComponent<Rigidbody>().constraints &= RigidbodyConstraints.FreezePositionX;
            this.gameObject.GetComponent<Rigidbody>().constraints &= RigidbodyConstraints.FreezePositionZ;
            this.gameObject.GetComponent<Rigidbody>().constraints &= RigidbodyConstraints.FreezePositionY;
        }
    }
}
