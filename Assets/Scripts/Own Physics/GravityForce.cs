using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForce : MonoBehaviour,IForce
{
    public float mass;
    public float g;
    public List<Vector3> CurrentForceVector { get; private set; }

    public List<Vector3> AbsolutePointOfForceApplying { get; private set; } 
    // Start is called before the first frame update
    void Start()
    {
        CurrentForceVector = new List<Vector3>() { Vector3.zero };
        AbsolutePointOfForceApplying = new List<Vector3>() { transform.position };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CountForce()
    {
        CurrentForceVector[0] = new Vector3(0, g * mass, 0);
        Vector3 pointOfApplication = Vector3.zero;
        AbsolutePointOfForceApplying[0] = transform.TransformPoint(pointOfApplication);
    }
}
