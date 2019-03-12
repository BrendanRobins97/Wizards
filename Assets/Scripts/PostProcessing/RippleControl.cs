using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleControl : MonoBehaviour
{
    private float displacementAmount;

    public MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        displacementAmount = Mathf.Lerp(displacementAmount, 0, Time.deltaTime);
        meshRenderer.material.SetFloat("_Amount", displacementAmount);
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Ripple");
            displacementAmount += 1;
        }
    }
}
