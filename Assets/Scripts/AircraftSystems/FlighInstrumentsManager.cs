using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlighInstrumentsManager : MonoBehaviour
{
    [SerializeField]
    private Transform gyroscope;
    [SerializeField]
    private EngineForce leftEngine;
    [SerializeField]
    private EngineForce rightEngine;
    [SerializeField]
    private PlaneController planeController;
    [SerializeField]
    private AircraftRadar radar;

    public bool ILS_captured { get { return radar.ILS_captured; } }
    public float Angle_ILS_hor { get { return radar.Angle_ILS_hor; } }
    public float Angle_ILS_ver { get { return radar.Angle_ILS_ver; } }

    public float PitchAngle { get
        {
            float pitch = gyroscope.transform.eulerAngles.x;
            if (pitch > 180)
                pitch -= 360f;
            return pitch;
        } }
    public float BankAngle
    {
        get
        {
            return -gyroscope.transform.eulerAngles.z;
        }
    }
    public float Altitude
    {
        get
        {
            return planeController.altitude;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            Debug.DrawLine(transform.position, transform.position + planeController.transform.TransformDirection(planeController.VelocityInLocalCoordinates)-MainManager.Instance.GetWind(planeController.transform.position), Color.cyan);
            return planeController.VelocityInLocalCoordinates;
        }
    }
    public float StabilizerTrimAngle
    {
        get
        {
            return planeController.StabilizerTrimAngle;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public float GetEngineLevel(int i)
    {
        if (i == 0)
            return leftEngine.NominalLevel;
        if (i == 1)
            return rightEngine.NominalLevel;
        return 0;
    }
}
