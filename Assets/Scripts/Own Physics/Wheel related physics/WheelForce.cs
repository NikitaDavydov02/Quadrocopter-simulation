using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum WheelStatus
{
    Roll,Slide,InAir,
}
public class WheelForce : MonoBehaviour, IForce
{
    
    public Vector3 globalAxisMomentum;
    private Vector3 reactionNormalForce; //absolute vector
    private Vector3 reactionNormalForcePoint; //absolute vector
    private Vector3 touchGroundPointVelocity; //absolute vector
    private Vector3 frictionForce;

    private Vector3 wheelCenterPoint;
    private Vector3 wheelRestCenterPoint;
    

    public WheelStatus status;
    
    [SerializeField]
    private float friction_coeff;
    [SerializeField]
    private float wheel_inertia_moment;
    [SerializeField]
    private float sliding_relative_tolerance;
    [SerializeField]
    private float R;
    [SerializeField]
    private float maxAngularSpeed=100f;
    [SerializeField]
    private float rest_distance_to_the_wheel_center = 1f;
    private float distance_to_the_wheel_center;
    [SerializeField]
    private float max_distance_to_the_wheel_center = 2f;
    [SerializeField]
    private float k_stiffness;
    [SerializeField]
    private float dumping_coeff;
    [SerializeField]
    private float force_filter = 0.01f;

    [SerializeField]
    private float brakingMoment=10f;
    [SerializeField]
    private float max_dx = 0.5f;
   
    // [SerializeField]
    // private float touchPointOffsetTolerance = 1.1f;

    private Vector3 slidingSpeed;
    public float angularVelocity;//aligned with Vector3.up
    private Vector3 touchRimPointVelocity; //absolute vector

    private bool braking = false;
    Vector3 force_abs;
    Vector3 force_point;
    Vector3 angular_velocity_abs;

    private Vector3 previous_force_spring;
    private Vector3 previoud_force_friction;


    MeshRenderer wheelMeshRenderer;
    private Rigidbody parent_rb;
    [SerializeField]
    Transform wheelTransform;

    private float time;
    private StreamWriter sw;
    // Start is called before the first frame update
    void OnDestroy()
    {
        //sw.Close();
    }
    void Start()
    {
        string logPath = "C:\\Users\\User\\Documents\\unity_log_" + gameObject.name + ".txt";
        //sw = new StreamWriter(File.Create("C:\\Users\\User\\Documents\\unity_log.txt"));
        //sw.WriteLine("Hi!");
        sw = new StreamWriter(new FileStream(logPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
        sw.AutoFlush = true;
        //sw.Close();
        parent_rb = transform.root.GetComponent<Rigidbody>();
        wheelMeshRenderer = wheelTransform.gameObject.GetComponent<MeshRenderer>();
        if (wheel_inertia_moment == 0)
            Debug.LogError("wheel_inertia_moment==0");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        sw.Write(time + ";");
        UpdateStatus();

        /*Vector3 wheelRestLocal = Vector3.down * distance_to_the_wheel_center;
        wheelTransform.localPosition = wheelRestLocal + transform.localPosition;
        Debug.Log("Wheel " + name + ": wheelRestLocal " + wheelRestLocal);
        Debug.Log("Wheel " + name + ": ray point local position " + transform.localPosition);*/
        wheelTransform.position = transform.position + Vector3.down * distance_to_the_wheel_center;

        //<ROTATE WHEEL>
        Vector3 r = force_point - wheelCenterPoint;
        Vector3 M = Vector3.Cross(r, frictionForce);
        Debug.DrawLine(wheelCenterPoint, wheelCenterPoint + globalAxisMomentum, Color.blue);
        Debug.Log("M_friction " + M );
        Debug.Log("M_engine: " + globalAxisMomentum.magnitude);
        Debug.Log("Dor(M_friction,M_engine):" + Vector3.Dot(M.normalized, globalAxisMomentum.normalized));
        if (!braking)
        {
            M += globalAxisMomentum;//COMMENT
            //parent_rb.AddForce(parent_rb.transform.TransformDirection(Vector3.forward) * globalAxisMomentum.magnitude);
        }
        if (braking)
        {
            Debug.Log("Braking");
            //calculate braking momentum
            M -= angular_velocity_abs.normalized * (brakingMoment);
            //angularVelocity = 0f;
        }
        Debug.DrawLine(wheelCenterPoint, wheelCenterPoint + M, Color.blue);
        angular_velocity_abs += M * Time.deltaTime / wheel_inertia_moment;
        angularVelocity = Vector3.Dot(angular_velocity_abs, wheelTransform.TransformDirection(Vector3.up));
        /*if (Vector3.Dot(M, transform.TransformDirection(Vector3.up))>0)
            angularVelocity -=(M.magnitude/wheel_inertia_moment)*Time.deltaTime;
        else
            angularVelocity += (M.magnitude / wheel_inertia_moment) * Time.deltaTime;*/
        if(angularVelocity > maxAngularSpeed)
            angularVelocity = maxAngularSpeed;
        if (angularVelocity < -maxAngularSpeed)
            angularVelocity = -maxAngularSpeed;
        //--------------------------------//
        wheelTransform.Rotate(Vector3.up, angularVelocity * Time.deltaTime*Mathf.Rad2Deg, Space.Self);
        //</ROTATE WHEEL>
        sw.WriteLine();
    }
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        force_abs = Vector3.zero;
        force_point = Vector3.zero;
        frictionForce = Vector3.zero;
        //Debug.DrawLine(transform.position, reactionNormalForcePoint, Color.green);
        if (reactionNormalForcePoint!=Vector3.zero)
        {
            
            force_point = reactionNormalForcePoint;
            force_abs += reactionNormalForce;
            if(status==WheelStatus.Slide)
            {
                //reactionNormalForce = Vector3.up * parent_rb.mass * 9.81f / 4f;//COMMENT
                frictionForce = -slidingSpeed.normalized * friction_coeff * reactionNormalForce.magnitude;
                //frictionForce.y = 0;//COMMENT
                //force_abs += frictionForce;
            }


        }
        if(force_abs.magnitude>50*parent_rb.mass)
        {
            //Debug.LogError("Exceeding force limit: " + force_abs);
            force_abs = Vector3.zero;
        }
        Debug.Log("Wheel force: " + force_abs);

        //<FILTER>
        /*if(previoud_force_friction!=Vector3.zero)
        {
            float rel_delta = (frictionForce - previoud_force_friction).magnitude / previoud_force_friction.magnitude;
            float scaling = 1.0f;
            if (rel_delta > force_filter)
                scaling = rel_delta / force_filter;
            frictionForce = previoud_force_friction + (frictionForce - previoud_force_friction) / scaling;
        }*/
        /* if (previous_force_spring != Vector3.zero)
         {
             float rel_delta = (force_abs - previous_force_spring).magnitude / previous_force_spring.magnitude;
             float scaling = 1.0f;
             if (rel_delta > force_filter)
                 scaling = rel_delta / force_filter;
             force_abs = previous_force_spring + (force_abs - previous_force_spring) / scaling;
         }*/
        //<FILTER>

        CurrentForceVectors = new List<Vector3>() { force_abs,frictionForce };
        AbsolutePointsOfForceApplying = new List<Vector3>() { transform.position, force_point };
        /*CurrentForceVectors = new List<Vector3>() { Vector3.zero };
        AbsolutePointsOfForceApplying = new List<Vector3>() { Vector3.zero };
        parent_rb.AddForceAtPosition(reactionNormalForce, transform.position);*/

        previous_force_spring = force_abs;
        previoud_force_friction = frictionForce;
    }
    float previous_d;
    float spring_velocity;
    public float CalculateSpringForce()
    {
        distance_to_the_wheel_center = (wheelCenterPoint - transform.position).magnitude;
        float d_d = distance_to_the_wheel_center - previous_d;
        spring_velocity = d_d / Time.deltaTime;//COMMENT
        //spring_velocity = Vector3.Dot(Vector3.up, parent_rb.GetPointVelocity(transform.position));
        if (previous_d == 0 || Mathf.Abs(spring_velocity)>max_distance_to_the_wheel_center/0.05f)
            spring_velocity = 0;
        previous_d = distance_to_the_wheel_center;


        float dx = distance_to_the_wheel_center - rest_distance_to_the_wheel_center;
        if (dx > max_distance_to_the_wheel_center + 2 * R)
            dx = 0;
        if (dx > max_dx)
            dx = max_dx;
        if (dx < -max_dx)
            dx = -max_dx;

        sw.Write(dx + ";");
        sw.Write(spring_velocity + ";");
        Debug.Log("Force_dx: " + dx);
        Debug.Log("Force_v: " + spring_velocity);
        return (dx * k_stiffness + spring_velocity * dumping_coeff);// + velocity * dumping_coeff);
    }
    public void UpdateStatus()
    {
        Debug.Log("Update status of whell entered");
        //<CHECK IN AIR>
        RaycastHit raycastHit;
        //int ignoreLayer = LayerMask.NameToLayer("IgnoreRaycast");
        //int ignoreMask = (1 << ignoreLayer);
        //int mask = ~ignoreMask;
        
        //int raycastLayer = LayerMask.GetMask("IgnoreRaycast");
        //int ignoreRaycastLayer = ~raycastLayer;

        int raycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        int ignoreRaycastLayer = ~(1 << raycastLayer);
        Debug.Log("raycat layer: " + raycastLayer);
        Debug.Log("ignore raycast layer: " + ignoreRaycastLayer);

        wheelRestCenterPoint = transform.position + transform.TransformDirection(Vector3.down) * rest_distance_to_the_wheel_center;
       
        Debug.Log("Wheel " + name + " position: " + wheelRestCenterPoint);
        Debug.DrawLine(parent_rb.position, wheelRestCenterPoint, Color.black);
        //if (Physics.Raycast(new Ray(transform.position,Vector3.down),out raycastHit,max_distance_to_the_wheel_center+R))
        if (Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.down)), out raycastHit, max_distance_to_the_wheel_center + R, ignoreRaycastLayer))
        {
            Debug.Log("raycast hit: " + raycastHit.collider.name);
            wheelCenterPoint = raycastHit.point - transform.TransformDirection(Vector3.down) * R;
            reactionNormalForcePoint = raycastHit.point;
            reactionNormalForce = Vector3.down * CalculateSpringForce();
           
            touchGroundPointVelocity = parent_rb.GetPointVelocity(raycastHit.point);
            touchGroundPointVelocity.y = 0;//COMMENT
            Debug.DrawLine(parent_rb.position, wheelCenterPoint, Color.black);
            Debug.DrawLine(parent_rb.position, raycastHit.point, Color.yellow);
        }
        else
        {
            reactionNormalForcePoint = Vector3.zero;
            reactionNormalForce = Vector3.zero;
            wheelCenterPoint = transform.position + transform.TransformDirection(Vector3.down) * rest_distance_to_the_wheel_center;
            status = WheelStatus.InAir;
            wheelMeshRenderer.material.color = Color.blue;
            Debug.DrawLine(parent_rb.position, wheelCenterPoint, Color.black);
            return;
        }


        //<UPDATE STATUS>
        Vector3 radius_abs = transform.TransformDirection(Vector3.down) * R;
        angular_velocity_abs = wheelTransform.TransformDirection(Vector3.up) * angularVelocity;
        Debug.DrawLine(wheelTransform.position, wheelTransform.position+angular_velocity_abs, Color.yellow);
        touchRimPointVelocity = Vector3.Cross(angular_velocity_abs, radius_abs);
        Debug.Log("Radius abs: " + radius_abs);
        Debug.DrawLine(reactionNormalForcePoint, reactionNormalForcePoint + touchRimPointVelocity, Color.cyan);
        Debug.DrawLine(reactionNormalForcePoint, reactionNormalForcePoint + touchGroundPointVelocity, Color.cyan);
        slidingSpeed = touchRimPointVelocity + touchGroundPointVelocity;
        Debug.DrawLine(reactionNormalForcePoint, reactionNormalForcePoint + slidingSpeed, Color.black);
        if (touchGroundPointVelocity.magnitude > 0.01f)
        {
           if (slidingSpeed.magnitude / touchGroundPointVelocity.magnitude > sliding_relative_tolerance)
           //if (slidingSpeed.magnitude> sliding_relative_tolerance)
            {
                status = WheelStatus.Slide;
                wheelMeshRenderer.material.color = Color.yellow;
            }
            else
            {
                status = WheelStatus.Roll;
                wheelMeshRenderer.material.color = Color.green;
            }
        }
        else
        {
            if (slidingSpeed.magnitude > 0.01f)
            {
                status = WheelStatus.Slide;
                wheelMeshRenderer.material.color = Color.yellow;
            }
            else
            {
                status = WheelStatus.Roll;
                wheelMeshRenderer.material.color = Color.green;
            }
        }
        //</UPDATE STATUS>
    }
    public void BrakeIn()
    {
        braking = true;
    }
    public void BrakeOut()
    {
        braking = false;
    }
    void OnApplicationQuit()
    {
        sw?.Close();
    }
}
