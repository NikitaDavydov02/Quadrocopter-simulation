using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLiftForce : MonoBehaviour, IForce
{
    [SerializeField]
    private float K;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        Vector3 force = Vector3.up * K * (rb.linearVelocity.magnitude) * (rb.linearVelocity.magnitude);
        CurrentForceVectors = new List<Vector3>() { force };
        AbsolutePointsOfForceApplying = new List<Vector3>() { rb.worldCenterOfMass };
    }
}
