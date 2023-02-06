using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WingForce : MonoBehaviour, IForce
{
    //public Vector3 NormalVectorInRelativeCoordinates;
    //public Vector3 forwardVectorInRelativeCoordinates;
    public float length;
    public float chord;
    public Vector3 velocity = Vector3.zero;
    private Vector3 lastPosition;
    public AnimationCurve CxDependenceVSAngleOfAtack;
    public AnimationCurve CyDependenceVSAngleOfAtack;
    private float area;
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>() { Vector3.zero };
        AbsolutePointsOfForceApplying = new List<Vector3>() { transform.position };
        //Vector3 normalvelocity = Vector3.Dot(velocity, NormalVector.normalized) * velocity;
        //Vector3 parallelVelocity = velocity - normalvelocity;
        Vector3 flowVelocityInSelfCoordinates = transform.InverseTransformDirection(-velocity);
        Vector3 flowVelocityInSelfCoordinatesWithoutTangent = new Vector3(0, flowVelocityInSelfCoordinates.y, flowVelocityInSelfCoordinates.z);
        float angleOfAtack = Mathf.Rad2Deg* Mathf.Atan2(-flowVelocityInSelfCoordinatesWithoutTangent.y, flowVelocityInSelfCoordinatesWithoutTangent.z);
        Debug.Log("Angle Of atack: " + angleOfAtack);
        float Cx = CxDependenceVSAngleOfAtack.Evaluate(angleOfAtack);
        float Cy = CyDependenceVSAngleOfAtack.Evaluate(angleOfAtack);
        float lift = (1 / 2) * MainManager.AirDensity * area* flowVelocityInSelfCoordinatesWithoutTangent.sqrMagnitude * Cy;
        float drag = (1 / 2) * MainManager.AirDensity * area * flowVelocityInSelfCoordinatesWithoutTangent.sqrMagnitude * Cx;
        Vector3 relativeDrag = drag * flowVelocityInSelfCoordinatesWithoutTangent.normalized;
        Vector3 liftDirectionInRelative = Vector3.Cross(transform.right, flowVelocityInSelfCoordinates.normalized);
        Vector3 relativeLift = lift * liftDirectionInRelative;
        Vector3 relativeSum = relativeDrag + relativeLift;
        Vector3 absoluteForce = transform.TransformDirection(relativeSum);
        Vector3 relativePointOfApplaing = new Vector3(0, 0, chord / 4);
        Vector3 absolutePointOfApplaing = transform.TransformDirection(relativePointOfApplaing);

        Debug.DrawLine(absolutePointOfApplaing, absolutePointOfApplaing + absoluteForce, Color.cyan);

        CurrentForceVectors.Add(absoluteForce);
        AbsolutePointsOfForceApplying.Add(absolutePointOfApplaing);
    }

    // Start is called before the first frame update
    void Start()
    {
        area = length * chord;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
}
