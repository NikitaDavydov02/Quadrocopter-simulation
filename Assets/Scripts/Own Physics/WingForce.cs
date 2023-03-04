using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WingForce : MonoBehaviour, IForce
{
    public float length;
    public float chord;
    public Vector3 velocity;
    private Vector3 lastPosition;
    public AnimationCurve CxDependenceVSAngleOfAtack;
    public AnimationCurve CyDependenceVSAngleOfAtack;
    private float area;
    public Rigidbody rb;
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        velocity = rb.velocity + Vector3.Cross(rb.angularVelocity, transform.position - rb.gameObject.transform.position);
        //velocity = rb.velocity;
        //Debug.Log("velocity" + gameObject.transform.parent.GetComponent<Rigidbody>().velocity);
        //Debug.DrawLine(transform.position, transform.position + velocity, Color.yellow);
        CurrentForceVectors = new List<Vector3>() { Vector3.zero };
        AbsolutePointsOfForceApplying = new List<Vector3>() { transform.position };
        Vector3 flowVelocityInSelfCoordinates = transform.InverseTransformDirection(-velocity);
        //Debug.Log("Flow velocity: " + -velocity);
        //Debug.Log("Flow velocity in SC: " + flowVelocityInSelfCoordinates);
        Vector3 flowVelocityInSelfCoordinatesWithoutTangent = new Vector3(0, flowVelocityInSelfCoordinates.y, flowVelocityInSelfCoordinates.z);
        //Debug.Log("vx: " + flowVelocityInSelfCoordinatesWithoutTangent.z);
        //Debug.Log("vy: " + flowVelocityInSelfCoordinatesWithoutTangent.y);
        float angleOfAtack = Mathf.Atan2(flowVelocityInSelfCoordinates.y, -flowVelocityInSelfCoordinates.z) * Mathf.Rad2Deg;
        if (flowVelocityInSelfCoordinatesWithoutTangent.magnitude < 0.1f)
            angleOfAtack = 0;
        //Debug.Log("Angle Of atack: " + angleOfAtack);
        float Cx = CxDependenceVSAngleOfAtack.Evaluate(angleOfAtack);
        float Cy = CyDependenceVSAngleOfAtack.Evaluate(angleOfAtack);
        //Debug.Log("Cx: " + Cx);
        //Debug.Log("Cy: " + Cy);
        float lift = MainManager.AirDensity * area* flowVelocityInSelfCoordinatesWithoutTangent.sqrMagnitude * Cy;
        float drag = MainManager.AirDensity * area * flowVelocityInSelfCoordinatesWithoutTangent.sqrMagnitude * Cx;
       
        Vector3 dragDirectionAbsolute = transform.TransformDirection(flowVelocityInSelfCoordinatesWithoutTangent).normalized;
        Vector3 liftDirectionInRelative = -Vector3.Cross(flowVelocityInSelfCoordinatesWithoutTangent, Vector3.right).normalized;
        Vector3 liftDirectionInAbsolute = transform.TransformDirection(liftDirectionInRelative).normalized;
        //Debug.Log("lift: " + lift);
        //Debug.Log("drag: " + drag);

        Vector3 dragAbsolute = dragDirectionAbsolute * drag;
        Vector3 liftAbsolute = liftDirectionInAbsolute * lift;
        //Debug.DrawLine(transform.position, transform.position + dragAbsolute,Color.black);
        //Debug.DrawLine(transform.position, transform.position + liftAbsolute, Color.black);

        Vector3 absoluteForce = dragAbsolute + liftAbsolute;
        Vector3 absolutePoint = transform.TransformPoint(0, 0, 0);
        CurrentForceVectors.Add(absoluteForce);
        AbsolutePointsOfForceApplying.Add(absolutePoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        area = length * chord;
        rb = gameObject.transform.parent.GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;

        //Debug.Log("V:" + velocity);
        lastPosition = rb.centerOfMass;
    }
}
