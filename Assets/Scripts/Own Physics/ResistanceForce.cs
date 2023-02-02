using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceForce : MonoBehaviour, IForce
{
    private float k;
    public Primitive Primitive;
    public float length;
    public float diameter;
    public List<Vector3> CurrentForceVector { get; private set; }

    public List<Vector3> AbsolutePointOfForceApplying { get; private set; }
    public Vector3 velocity = Vector3.zero;
    private Vector3 lastPosition;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        CurrentForceVector = new List<Vector3>() { Vector3.zero };
        AbsolutePointOfForceApplying = new List<Vector3>() { transform.position };
        lastPosition = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
    public void CountForce()
    {
        Debug.Log("velostiy: " + velocity);
        if (velocity.magnitude == 0)
        {
            CurrentForceVector[0] = Vector3.zero;
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
        //float module = MainManager.AirDensity * velocity.magnitude *velocity.magnitude * k*area / 2;
        CurrentForceVector[0]= forceInGlobalCoordinates;
        Vector3 pointOfApplication = Vector3.zero;
        AbsolutePointOfForceApplying[0]=transform.TransformPoint(pointOfApplication);
    }
}
public enum Primitive {
    Stick,
}

