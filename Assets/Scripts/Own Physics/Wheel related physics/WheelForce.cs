using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WheelStatus
{
    Roll,Slide,InAir,
}
public class WheelForce : MonoBehaviour, IForce
{
    public Vector3 reactionNormalForce; //absolute vector
    public Vector3 reactionNormalForcePoint; //absolute vector
    public Vector3 touchGroundPointVelocity; //absolute vector
    public bool reactionForceWasUpdated
    {
        set { noTouchIterationsCounter = 0; }
    }

    public WheelStatus status;
    
    [SerializeField]
    private float friction_coeff;
    [SerializeField]
    private float wheel_inertia_moment;
    [SerializeField]
    private float sliding_relative_tolerance;
    [SerializeField]
    private float R;
   // [SerializeField]
   // private float touchPointOffsetTolerance = 1.1f;

    private Vector3 slidingSpeed;
    public float angularVelocity;//aligned with Vector3.up
    private Vector3 touchRimPointVelocity; //absolute vector

    private int noTouchIterationsCounter;
    

    Vector3 force_abs;
    Vector3 force_point;
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (wheel_inertia_moment == 0)
            Debug.LogError("wheel_inertia_moment==0");
    }

    // Update is called once per frame
    void Update()
    {
        noTouchIterationsCounter++;
        UpdateStatus();
        //<ROTATE WHEEL>
        Vector3 r = force_point - transform.position;
        Vector3 M = Vector3.Cross(r, force_abs);
        if(Vector3.Dot(M, transform.TransformDirection(Vector3.up))>0)
            angularVelocity +=(M.magnitude/wheel_inertia_moment)*Time.deltaTime;
        else
            angularVelocity -= (M.magnitude / wheel_inertia_moment) * Time.deltaTime;
        //--------------------------------//
        transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime*Mathf.Rad2Deg, Space.Self);
        //</ROTATE WHEEL>
    }
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        force_abs = Vector3.zero;
        force_point = Vector3.zero;
        if(status==WheelStatus.Slide && reactionNormalForcePoint!=Vector3.zero && reactionNormalForcePoint!=null)
        {
            force_point = reactionNormalForcePoint;
            Debug.Log("Force_point: " + reactionNormalForcePoint);
            force_abs = -slidingSpeed.normalized * friction_coeff* reactionNormalForce.magnitude;
        }
        
        CurrentForceVectors = new List<Vector3>() { force_abs };
        AbsolutePointsOfForceApplying = new List<Vector3>() { force_point };
    }
    public void UpdateStatus()
    {
        Debug.Log("Update status of whell entered");
        //<CHECK IN AIR>
        if(noTouchIterationsCounter>2)
        //if((transform.position-reactionNormalForcePoint).magnitude>touchPointOffsetTolerance*R)
        //<CHECK IN AIR>
        //if (reactionNormalForcePoint == null || touchGroundPointVelocity == null|| reactionNormalForcePoint == Vector3.zero || touchGroundPointVelocity == Vector3.zero)
        {
            status = WheelStatus.InAir;
            meshRenderer.material.color = Color.blue;
            return;
        }
        //<REFINING PRECISE TOUCH POINT>
        Vector3 r_vector = reactionNormalForce.normalized * (-R);
        reactionNormalForcePoint = transform.position + r_vector;
        //<REFINING PRECISE TOUCH POINT>

        //<UPDATE STATUS>
        Vector3 radius_abs = reactionNormalForcePoint - transform.position;
        Vector3 angular_velocity_abs = transform.TransformDirection(Vector3.up) * angularVelocity;
        touchRimPointVelocity = Vector3.Cross(angular_velocity_abs, radius_abs);
        Debug.Log("Radius abs: " + radius_abs);
        Debug.DrawLine(transform.position + radius_abs, transform.position + radius_abs + touchRimPointVelocity, Color.cyan);
        Debug.DrawLine(transform.position + radius_abs, transform.position + radius_abs + touchGroundPointVelocity, Color.cyan);
        slidingSpeed = touchRimPointVelocity + touchGroundPointVelocity;
        Debug.DrawLine(transform.position + radius_abs, transform.position + radius_abs + slidingSpeed, Color.black);
        if (touchGroundPointVelocity.magnitude > 0.01f)
        {
            if (slidingSpeed.magnitude / touchGroundPointVelocity.magnitude > sliding_relative_tolerance)
            {
                status = WheelStatus.Slide;
                meshRenderer.material.color = Color.yellow;
            }
            else
            {
                status = WheelStatus.Roll;
                meshRenderer.material.color = Color.green;
            }
        }
        //</UPDATE STATUS>
    }

   
}
