using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressistanceforce : MonoBehaviour
{
    // Start is called before the first frame update
    public float area;
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.AddForce(CountResistance(), ForceMode.Force);
    }
    public Vector3 CountResistance()
    {
        Vector3 direction = -rb.velocity / rb.velocity.magnitude;
        float module = MainManager.AirDensity * rb.velocity.magnitude * rb.velocity.magnitude * area / 2;
        return direction * module;
    }

}
