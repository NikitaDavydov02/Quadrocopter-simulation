using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftRadar : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject capturedILS;
    private float verticalShiftDegrees;
    private float horizontalShiftDegrees;
    float angle_ILS_hor;
    float angle_ILS_ver;
    public bool ILS_captured { get { if (capturedILS != null) return true; else return false; } }
    public float Angle_ILS_hor { get { if (capturedILS) return angle_ILS_hor; else return float.NaN; } }
    public float Angle_ILS_ver { get { if (capturedILS) return angle_ILS_ver; else return float.NaN; } }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] ILSes = GameObject.FindGameObjectsWithTag("ILS");
        if (ILSes.Length != 0)
        {
            capturedILS = ILSes[0];
        }
        if (capturedILS != null)
        {
            Vector3 planeCourse = transform.TransformDirection(Vector3.forward);
            Vector3 fromPlaneToILS = capturedILS.transform.position - transform.position;
            Vector3 ILS_direction = capturedILS.transform.TransformDirection(Vector3.forward);
            if (Vector3.Dot(ILS_direction, planeCourse) < 0f)
            {
                Vector3 fromPlaneToILS_relative = transform.InverseTransformDirection(fromPlaneToILS);
                Vector3 fromPlaneToILS_relative_hor = fromPlaneToILS_relative;
                fromPlaneToILS_relative_hor.y = 0;
                Vector3 fromPlaneToILS_relative_ver = fromPlaneToILS_relative;
                fromPlaneToILS_relative_ver.x = 0;
                angle_ILS_hor = Vector3.Angle(Vector3.forward, fromPlaneToILS_relative_hor);
                if (fromPlaneToILS_relative_hor.x < 0)
                    angle_ILS_hor *= -1;
                angle_ILS_ver = Vector3.Angle(Vector3.forward, fromPlaneToILS_relative_ver);
                if (fromPlaneToILS_relative_ver.y < 0)
                    angle_ILS_ver *= -1;
                if (Mathf.Abs(angle_ILS_hor) > 40f || Mathf.Abs(angle_ILS_ver) > 40f)
                    capturedILS = null;
            }
            else
                capturedILS = null;
        }
    }
}
