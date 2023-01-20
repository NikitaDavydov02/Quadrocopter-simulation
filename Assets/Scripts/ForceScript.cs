using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceScript : MonoBehaviour
{
    public Vector3 CurrentLocalForce { get; private set; }
    public float MaxForce;
    public float level;
    public Vector3 AxisDirection;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rb.velocity);
    }
    private void LateUpdate()
    {
        AddEngineForce();
    }
    public void AddEngineForce()
    {
        Vector3 force =AxisDirection* MaxForce*level;
        force = transform.TransformDirection(force);
        rb.AddForce(force,ForceMode.Force);
    }
    public void AddAngularMomentum()
    {
        //rb.AddRelativeTorque()

        Vector3 force = AxisDirection * MaxForce * level;
        force = transform.TransformDirection(force);
        rb.AddForce(force, ForceMode.Force);
    }
}
