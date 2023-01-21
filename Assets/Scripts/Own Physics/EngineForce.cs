using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineForce : MonoBehaviour, IForce
{
    public List<Vector3> CurrentForceVector { get; private set; }

    public List<Vector3> AbsolutePointOfForceApplying { get; private set; }

    public Vector3 AxisDirection;
    public float MaxForce;
    public float Level;
    public float AxisRadius = 0.1f;
    public float RotationCoeffitient = 1f;

    Vector3 firstRotationForceRelativePoint;
    Vector3 secondRotationForceRelativePoint;
    Vector3 firstRotationForceRelative;
    Vector3 secondRotationForceRelative;

    // Start is called before the first frame update
    void Start()
    {
        CurrentForceVector = new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero };
        firstRotationForceRelativePoint = new Vector3(AxisRadius, 0, 0);
        secondRotationForceRelativePoint = new Vector3(-AxisRadius, 0, 0);
        AbsolutePointOfForceApplying = new List<Vector3>() { transform.position, transform.TransformPoint(firstRotationForceRelativePoint), transform.TransformPoint(secondRotationForceRelativePoint)};

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountForce()
    {
        Vector3 force = AxisDirection * MaxForce * Level;
        force = transform.TransformDirection(force);
        CurrentForceVector[0] = force;
        Vector3 pointOfApplication = Vector3.zero;
        AbsolutePointOfForceApplying[0] = transform.TransformPoint(pointOfApplication);
        //Rotation forces
        Vector3 firstRotationForceRelative = RotationCoeffitient*MaxForce * Level * new Vector3(0, 0, 1);
        Vector3 secondRotationForceRelative = -firstRotationForceRelative;
        //CurrentForceVector[1] = transform.TransformDirection(firstRotationForceRelative);
        //CurrentForceVector[2] = transform.TransformDirection(secondRotationForceRelative);
        CurrentForceVector[1] = Vector3.zero;
        CurrentForceVector[2] = Vector3.zero;
        AbsolutePointOfForceApplying[1] = transform.TransformPoint(firstRotationForceRelativePoint);
        AbsolutePointOfForceApplying[2] = transform.TransformPoint(secondRotationForceRelativePoint);
    }
}
