using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceForce : MonoBehaviour, IForce
{
    private float k;
    public Primitive Primitive;
    public float length;
    public float diameter;
    //public List<Vector3> CurrentForceVector { get; private set; }

    //public List<Vector3> AbsolutePointOfForceApplying { get; private set; }
    public Vector3 velocity = Vector3.zero;
    private Vector3 lastPosition;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //CurrentForceVector = new List<Vector3>() { Vector3.zero };
        //AbsolutePointOfForceApplying = new List<Vector3>() { transform.position };
        lastPosition = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>();
        AbsolutePointsOfForceApplying = new List<Vector3>();
        Debug.Log("velostiy: " + velocity);
        if (velocity.magnitude == 0)
        {
            CurrentForceVectors.Add(Vector3.zero);
            AbsolutePointsOfForceApplying.Add(transform.position);
            return;
        }
        //Vector3 direction = -velocity / velocity.magnitude;
        float area = 0;
        Vector3 forceInGlobalCoordinates = Vector3.zero;
        if (Primitive == Primitive.Stick)
        {
            //X axis is parellel ti the stick
            Vector3 stickAxis = transform.TransformDirection(1, 0, 0);
            Vector3 perpendicularVelosity = velocity - Vector3.Dot(velocity, stickAxis) * stickAxis;
            Vector3 direction = -perpendicularVelosity.normalized;
            Debug.DrawLine(transform.TransformPoint(Vector3.zero), transform.TransformPoint(Vector3.zero) + direction, Color.grey);
            area = diameter * length;
            float k = 0.4f;
            float module = MainManager.AirDensity * perpendicularVelosity.magnitude * perpendicularVelosity.magnitude * k * area / 2;
            forceInGlobalCoordinates = direction * module;
        }
        if (Primitive == Primitive.Disk)
        {
            Vector3 diskAxis = transform.TransformDirection(0, 1, 0);
            Vector3 perpendicularVelosity = Vector3.Dot(velocity, diskAxis) * diskAxis;
            Vector3 direction = -perpendicularVelosity.normalized;
            Debug.DrawLine(transform.TransformPoint(Vector3.zero), transform.TransformPoint(Vector3.zero) + diskAxis, Color.yellow);
            area = Mathf.PI*diameter * diameter/4;
            float k = 1.15f;
            float module = MainManager.AirDensity * perpendicularVelosity.magnitude * perpendicularVelosity.magnitude * k * area / 2;
            forceInGlobalCoordinates = direction * module;
        }
        //float module = MainManager.AirDensity * velocity.magnitude *velocity.magnitude * k*area / 2;
        CurrentForceVectors.Add(forceInGlobalCoordinates);
        Vector3 pointOfApplication = Vector3.zero;
        AbsolutePointsOfForceApplying.Add(transform.TransformPoint(pointOfApplication));
    }
}
public enum Primitive {
    Stick,
    Disk,
}

