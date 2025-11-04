using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform steeringPart;
    [SerializeField]
    private float steeringSensitivity;
    [SerializeField]
    private float maxSteeringAngle;
    private float steeringAngle;
    private bool active = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SteeringInput(float steeringInput)
    {
        if (!active)
            return;
        steeringPart.Rotate(0, steeringInput, 0, Space.Self);
        steeringAngle += steeringInput;
        float correction = 0;
        if (steeringAngle > maxSteeringAngle)
            correction = maxSteeringAngle - steeringAngle;
        if (steeringAngle < -maxSteeringAngle)
            correction = -maxSteeringAngle - steeringAngle;
        steeringPart.Rotate(0, correction, 0, Space.Self);
        steeringAngle += correction;
    }
    public void DisableSteeringControl()
    {
        active = false;
        steeringPart.Rotate(0, -steeringAngle, 0, Space.Self);
        steeringAngle = 0;
    }
    public void EnableSteeringControl()
    {
        active = true;
    }
}
