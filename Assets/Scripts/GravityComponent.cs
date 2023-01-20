using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector3(0, -9.81f*rb.mass, 0));
    }
}
