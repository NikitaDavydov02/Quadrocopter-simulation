using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : ForceCalculationManager
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddRelativeTorque(0, 1, 0, ForceMode.Force);
    }
}
