using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceForce : MonoBehaviour, IForce
{
    public float area;
    public float k;
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
        Vector3 direction = -velocity / velocity.magnitude;
        float module = MainManager.AirDensity * velocity.magnitude *velocity.magnitude * k*area / 2;
        CurrentForceVector[0]= direction * module;
        Vector3 pointOfApplication = Vector3.zero;
        AbsolutePointOfForceApplying[0]=transform.TransformPoint(pointOfApplication);
    }
}
