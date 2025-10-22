using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBase : ForceCalculationManager
{
    /* [SerializeField]
     float upForce;
     [SerializeField]
     Vector3 forceOffset;*/
    //Rigidbody rb;
    [SerializeField]
    BasicLiftForce basicLift;
    [SerializeField]
    ResistanceForce resistanceForce;
    [SerializeField]
    List<WheelForce> wheelForces;
    [SerializeField]
    EngineForce engForce;
    [SerializeField] 
    float gasIncrementSpeed;
    [SerializeField]
    List<Transform> steeredWheels;
    [SerializeField]
    private float steeringSensitivity;
    
    public Vector3 centerOfMassLocal;
    public Vector3 inertiaTensor;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        Init();
       rb.centerOfMass = centerOfMassLocal;
        Debug.Log("Inertia tensor" + rb.inertiaTensor);
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;

        if (resistanceForce!=null)
            forceSources.Add(resistanceForce);
        foreach (IForce wheel in wheelForces)
            forceSources.Add(wheel);
        
        if(engForce!=null)
            forceSources.Add(engForce);
        if (basicLift != null)
            forceSources.Add(basicLift);
    }

    // Update is called once per frame
    void Update()
    {

        /* Vector3 forcePosition = transform.position + transform.TransformVector(forceOffset);
         Debug.DrawLine(transform.position, forcePosition, Color.green);
         rb.AddForceAtPosition(Vector3.up * upForce, forcePosition);*/
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (WheelForce wheel in wheelForces)
                wheel.BrakeIn();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (WheelForce wheel in wheelForces)
                wheel.BrakeOut();
        }
        if (engForce != null)
        {
            /*if (Input.GetKey(KeyCode.W))
                engForce.Level += gasIncrementSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                engForce.Level -= gasIncrementSpeed * Time.deltaTime;
            if (engForce.Level > 1)
                engForce.Level = 1;
            if (engForce.Level < 0)
                engForce.Level = 0;*/
        }
        float steeringInput = Input.GetAxis("Mouse X") * Time.deltaTime * steeringSensitivity;
        foreach(Transform t in steeredWheels)
        {
            t.Rotate(Vector3.up, steeringInput, Space.Self);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        /*Vector3 reactionForce = collision.impulse/ Time.fixedDeltaTime;
        
        foreach(ContactPoint point in collision.contacts)
        {
            WheelForce wheelForce = point.thisCollider.gameObject.GetComponent<WheelForce>();
            if (wheelForce!=null)
            {
                
                wheelForce.reactionNormalForcePoint = point.point;
                wheelForce.reactionForceWasUpdated = true;
                Debug.Log("Wheel contact: " + point.thisCollider.gameObject.name);
                Debug.Log("Wheel contact point: " + point.point);
                Vector3 touchPointVelocity = rb.GetPointVelocity(wheelForce.reactionNormalForcePoint);
                wheelForce.touchGroundPointVelocity = touchPointVelocity;
                wheelForce.reactionNormalForce = reactionForce;
                Debug.DrawLine(wheelForce.reactionNormalForcePoint, wheelForce.reactionNormalForcePoint + reactionForce, Color.red);
            }
        }*/
        
    }
}
