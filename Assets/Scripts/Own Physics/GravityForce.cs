using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForce : MonoBehaviour,IForce
{
    public float mass;
    public float g;
    public Vector3 CenterOfMass;
    //public List<Vector3> CurrentForceVector { get; private set; }

    //public List<Vector3> AbsolutePointOfForceApplying { get; private set; } 
    // Start is called before the first frame update
    void Start()
    {
        //CurrentForceVector = new List<Vector3>() { Vector3.zero };
        //AbsolutePointOfForceApplying = new List<Vector3>() { transform.position };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>() { Vector3.zero };
        AbsolutePointsOfForceApplying = new List<Vector3>() { transform.position };
        CurrentForceVectors[0] = new Vector3(0, g * mass, 0);
        Vector3 pointOfApplication = CenterOfMass;
        AbsolutePointsOfForceApplying[0] = transform.TransformPoint(pointOfApplication);
    }
}
