using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionJoint : MonoBehaviour
{
    /* [SerializeField]
     ForceCalculationManager rb_1;
     [SerializeField]
     ForceCalculationManager rb_2;*/
    [SerializeField]
    Rigidbody rb_1;
    [SerializeField]
    Rigidbody rb_2;
    [SerializeField]
    private Vector3 body_1_offset;
    [SerializeField]
    private Vector3 body_2_offset;
    [SerializeField]
    private float neitralLength;
    [SerializeField]
    private float k;
    [SerializeField]
    private float dumping_coeff;//N*s/m
    public float velocity;
    //[SerializeField]
    //private Vector3 rb_main_suspension_direction;
    // Start is called before the first frame update
    private float previous_d;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point_1 = rb_1.transform.position + rb_1.transform.TransformDirection(body_1_offset);
        Vector3 point_2 = rb_2.transform.position + rb_2.transform.TransformDirection(body_2_offset);
        float d = (point_2 - point_1).magnitude;
        float d_d = d - previous_d;
        previous_d = d;
        velocity = d_d / Time.deltaTime;

        float dx = d - neitralLength;
        Vector3 force_1_on_2 = (point_1 - point_2).normalized *( dx * k + velocity*dumping_coeff);


        //rb_2.AddForce(force_1_on_2, point_2);
        //rb_1.AddForce(-force_1_on_2, point_1);
        rb_2.AddForceAtPosition(force_1_on_2, point_2);
        rb_1.AddForceAtPosition(-force_1_on_2, point_1);
    }
}
