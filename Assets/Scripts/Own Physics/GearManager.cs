using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour
{
    // Start is called before the first frame update
    /*[SerializeField]
    private Transform upper_leg;
    [SerializeField]
    private Transform lower_leg;
    [SerializeField]
    private Transform gear;
    [SerializeField]
    private Transform upper_hinge;
    [SerializeField]
    private Transform middle_hinge;
    [SerializeField]
    private Transform lower_hinge;
    [SerializeField]
    private Transform gear_axis;*/

    [SerializeField]
    private List<WheelForce> wheels;
    [SerializeField]
    private float extensionTime;
    public bool extended { get; private set; }
    public bool inProgress { get; private set; }
    private float elapsedTime = 0f;

    [SerializeField]
    List<Animator> gearAnimators;

    [SerializeField]
    private SteeringManager steeringManager;

    [SerializeField]
    private GearsAudioManager gearsAudioManager;
    void Start()
    {
        extended = true;
        inProgress = false;
        /*gear_init_rotation = gear.localRotation;
        Quaternion rotation = Quaternion.AngleAxis(maxExtensionAngle, Vector3.forward);
        gear_extended_rotation = gear_init_rotation * rotation;
        upperLegLength = (upper_hinge.position - middle_hinge.position).magnitude;
        lowerLegLength = (lower_hinge.position - middle_hinge.position).magnitude;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (inProgress)
            elapsedTime += Time.deltaTime;
        if (elapsedTime > extensionTime)
        {
            inProgress = false;
            foreach (Animator animator in gearAnimators)
            {
                animator.SetBool("inProgress", inProgress);
            }
            if (extended)
            {
                foreach (WheelForce wheel in wheels)
                {
                    wheel.active = true;
                }
            }

        }
            
    }
    /*private void CalculateMiddleHingePosition()
    {
        float d = (upper_hinge.position - lower_hinge.position).magnitude;
        float cos_a = (upperLegLength * upperLegLength + lowerLegLength * lowerLegLength - d * d) / (2 * lowerLegLength * upperLegLength);
        Debug.Log("gears cos_a: " + cos_a);
        float sin_a = Mathf.Sqrt(1 - cos_a * cos_a);
        float l2_projection = upperLegLength * sin_a;
        float sin_lower = l2_projection / d;
        float cos_lower = Mathf.Sqrt(1 - sin_lower* sin_lower);
        Debug.Log("gears sin_lower: " + sin_lower);
        Vector3 e_down_to_up = (upper_hinge.position - lower_hinge.position).normalized;
        Vector3 down_to_axis = (gear_axis.position - lower_hinge.position);
        Vector3 e_down_to_axis_perp = down_to_axis - e_down_to_up * Vector3.Dot(e_down_to_up, down_to_axis);
        e_down_to_axis_perp = e_down_to_axis_perp.normalized;
        Vector3 middle_hidge_offset = sin_lower * e_down_to_axis_perp + cos_lower * e_down_to_up;
        Debug.DrawLine(lower_hinge.position, lower_hinge.position + middle_hidge_offset, Color.yellow);
    }
    private IEnumerator ExtendGears()
    {
        float elapsedTime = 0;
        while (elapsedTime < extensionTime)
        {
            Quaternion left = Quaternion.Slerp(gear_init_rotation, gear_extended_rotation, elapsedTime / extensionTime);
            gear.localRotation = left;
            elapsedTime += Time.deltaTime;
            CalculateMiddleHingePosition();
            yield return null;
        }

    }*/
    public void ToggleGears()
    {
        if (inProgress)
            return;
        inProgress = true;
        elapsedTime = 0f;
        extended = !extended;
        if (extended)
        {
            steeringManager.EnableSteeringControl();
            gearsAudioManager.GearsDown();
        }
        if (!extended)
        {
            steeringManager.DisableSteeringControl();
            gearsAudioManager.GearsUp();
        }
        foreach(Animator animator in gearAnimators)
        {
            animator.SetBool("extend", extended);
            animator.SetBool("inProgress", inProgress);
        }
        if (!extended)
        {
            foreach (WheelForce wheel in wheels)
            {
                wheel.active = false;
                wheel.angularVelocity = 0f;
            }
        }
    }
}
