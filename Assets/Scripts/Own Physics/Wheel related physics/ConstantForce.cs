using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantForce : MonoBehaviour, IForce
{
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    Vector3 relativeForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        Vector3 absoluteForce = transform.TransformVector(relativeForce);
        Vector3 absolutePointOfApplication = transform.position + transform.TransformVector(offset);
        CurrentForceVectors = new List<Vector3>() { absoluteForce };
        AbsolutePointsOfForceApplying = new List<Vector3>() { absolutePointOfApplication };
       
    }
}
