using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour
{
    [SerializeField]
    Transform leftElleron;
    [SerializeField]
    Transform rightElleron;
    private float maxEleronAngle;
    private PlaneController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlaneController>();
        maxEleronAngle = controller.maxEleronAngle;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = transform.localEulerAngles.z;
        if (angle > 180)
            angle = angle -360;
        Debug.Log("Angle:"+angle);
        if (angle > 5)
        {
            Debug.Log("Turn to the right");
            leftElleron.localEulerAngles=new Vector3(-maxEleronAngle, 0, 0);
            rightElleron.localEulerAngles = new Vector3(maxEleronAngle, 0, 0);
        }
        if (angle < -5)
        {
            Debug.Log("Turn to the left");
            leftElleron.localEulerAngles = new Vector3(maxEleronAngle, 0, 0);
            rightElleron.localEulerAngles = new Vector3(-maxEleronAngle, 0, 0);
        
        }
    }
}
