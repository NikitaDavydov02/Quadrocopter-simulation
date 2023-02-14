using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeTorque(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
       // rb.AddForce(0, 1, 0);
    }
}
