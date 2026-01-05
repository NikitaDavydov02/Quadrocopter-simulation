using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CesiumForUnity;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class PlaneController : ForceCalculationManager
{
    public float altitude;
    public float density=1f;
    CesiumGlobeAnchor anchor;
    public float generalLevel = 0;
    public float generalLevelChangingSpeed = 1f;
    List<float> engineLevels = new List<float>();
    public float maxEleronAngle = 2;
    public bool ControlActive = true;
    //public bool reverseIsOn = false;
    //public float engineReverseLevel = -0.2f;
    public float spoilerAngularSpeed = 90f;
    public float spoilerCurrentAngle = 0;
    //public float flapsAngularSpeed = 5f;
   // [SerializeField]
   // Transform steeringPart;
    [SerializeField]
    private float steeringSensitivity;
    //[SerializeField]
    //private float maxSteeringAngle;
    // private float steeringAngle;
    [SerializeField]
    private SteeringManager steeringManager;
    [SerializeField]
    List<WheelForce> wheels;
    [SerializeField]
    List<EngineForce> engines;
    [SerializeField]
    public GravityForce gravityForce;
    [SerializeField]
    public List<WingForce> wings;
    [SerializeField]
    List<ResistanceForce> resistanceForces;
    [SerializeField]
    List<Transform> heightController;
    [SerializeField]
    Transform horizontalController;
    [SerializeField]
    Transform leftElleron;
    [SerializeField]
    Transform rightElleron;
    [SerializeField]
    Transform leftFlap;
    [SerializeField]
    Transform rightFlap;
    [SerializeField]
    Transform leftSpoiler;
    [SerializeField]
    Transform rightSpoiler;
    [SerializeField]
    private Transform horizontalStabilizer;
    [SerializeField]
    private float maxTrimAngle = 30f;
    [SerializeField]
    private float minTrimAngle = -5f;
    [SerializeField]
    private float trimRate = 2f;
    private float currentTrimAngle;

   // [SerializeField]
   // TMP_Text accelText;
    double lastVelocity = 0f;

    public float flaps = 0;
    public float flapsStep = 0.2f;
    public float eleronAngle = 0;
    public float eleronSensitivity;


    public float RotationPowerMultiplyer;
    public float gasSensitivity;
    public float heigtSensitivity;
    public float heightAngle = 0;
    public float horizontalSensitivity;
    public float horizontAngle = 0;
    public Vector3 centerOfMassLocal;
    public Vector3 inertiaTensor;
    public float maxHeightAngle = 10;
    public float maxHorizontalAngle = 10;
    public bool gearsUp = false;


    public Vector3 VelocityInLocalCoordinates = Vector3.zero;
    public Vector3 AngularVelocityInLocalCoordinates = Vector3.zero;

    public int currentProfile = 0;
    [SerializeField]
    public List<WingProfile> wingProfiles;
    [SerializeField]
    private GearsAudioManager gearsAudioManager;

    [SerializeField]
    private ReverseManager reverseManager;
    [SerializeField]
    private SpoilersManager spoilersManager;
    [SerializeField]
    private List<FlapsManager> flapsManagers;
    [SerializeField]
    private AileronManager aileronManager;
    [SerializeField]
    private GearManager gearManager;
    [SerializeField]
    private AircraftLightManager lightManager;

    public float StabilizerTrimAngle
    {
        get
        {
            float currentAngle = horizontalStabilizer.localEulerAngles.x;
            if (currentAngle > 180f)
                currentAngle -= 360f;
            return currentAngle;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anchor = gameObject.GetComponent<CesiumGlobeAnchor>();
        Init();
        rb.centerOfMass = centerOfMassLocal;
        Debug.Log("Inertia tensor" + rb.inertiaTensor);
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;

        //lastPosition = transform.position;
        generalLevel = 0;
        for (int i = 0; i < engines.Count; i++)
        {
            engineLevels.Add(0);
            forceSources.Add(engines[i]);
        }
        foreach (ResistanceForce f in resistanceForces)
            forceSources.Add(f);
        foreach (WingForce w in wings)
            forceSources.Add(w);
        foreach (WheelForce w in wheels)
            forceSources.Add(w);
        //forceSources.Add(gravityForce);
        if (leftSpoiler != null)
        {
            leftSpoiler.GetComponent<WingForce>().degree = 0;
            rightSpoiler.GetComponent<WingForce>().degree = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(anchor!=null)
            altitude = (float)anchor.height;
        density = MainManager.Instance.GetAirDensity(altitude);
        foreach (WingForce wf in wings)
            wf.density = density;
        foreach (ResistanceForce rf in resistanceForces)
            rf.density = density;
        rb.centerOfMass = centerOfMassLocal;
        //if (accelText!=null)
        //    accelText.text = ((rb.velocity.magnitude - lastVelocity)/ Time.deltaTime).ToString("0.00");
        lastVelocity = rb.linearVelocity.magnitude;

        Debug.Log("Inertia tensor" + rb.inertiaTensor);
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;
        rb.inertiaTensorRotation = Quaternion.Euler(0,0,0);
        CountState();
        if (Input.GetKey(KeyCode.W) && ControlActive)
            generalLevel += generalLevelChangingSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) && ControlActive)
            generalLevel -= generalLevelChangingSpeed * Time.deltaTime;
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < 0)
            generalLevel = 0;
        for (int i = 0; i < engineLevels.Count; i++)
            engineLevels[i] = generalLevel;
        float vwrticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * heigtSensitivity;
        if (!ControlActive)
            vwrticalInput = 0;
        //Debug.Log("Input:" + vwrticalInput);
        //Debug.Log("Euler:" + heightController.localEulerAngles.x);

        foreach (Transform t in heightController)
            t.Rotate(vwrticalInput, 0, 0, Space.Self);

        heightAngle += vwrticalInput;
        Debug.Log("Height angle: " + heightAngle);
        if (heightAngle < -maxHeightAngle)
        {
            foreach (Transform t in heightController)
                t.Rotate(-vwrticalInput, 0, 0, Space.Self);
            heightAngle -= vwrticalInput;
            Debug.Log("Height angle min");
        }
        if (heightAngle > maxHeightAngle)
        {
            foreach (Transform t in heightController)
                t.Rotate(-vwrticalInput, 0, 0, Space.Self);
            heightAngle -= vwrticalInput;
            Debug.Log("Height angle max");
        }
            
        float horInput = -Input.GetAxis("Mouse X") * Time.deltaTime * horizontalSensitivity;
        if (!ControlActive)
            horInput = 0;
        horizontalController.Rotate(horInput, 0, 0, Space.Self);
        horizontAngle += horInput;
        if (horizontAngle < -maxHorizontalAngle || horizontAngle > maxHorizontalAngle)
        {
            horizontalController.Rotate(-horInput, 0, 0, Space.Self);
            horizontAngle -= horInput;
        }

        float steeringInput = Input.GetAxis("Mouse X") * Time.deltaTime * steeringSensitivity;
        steeringManager.SteeringInput(steeringInput);
        //steeringPart.Rotate(0, steeringInput, 0, Space.Self);
        //steeringAngle += steeringInput;
        //float correction = 0;
        //if (steeringAngle > maxSteeringAngle)
        //    correction = maxSteeringAngle - steeringAngle;
        //if (steeringAngle < -maxSteeringAngle)
        //    correction = -maxSteeringAngle - steeringAngle;
        //steeringPart.Rotate(0, correction, 0, Space.Self);
        //steeringAngle += correction;

        if (Input.GetKeyDown(KeyCode.B) && ControlActive)
        {
            foreach (WheelForce w in wheels)
                w.BrakeIn();
        }
        if (Input.GetKeyUp(KeyCode.B) && ControlActive)
        {
            foreach (WheelForce w in wheels)
                w.BrakeOut();
        }

        if (Input.GetKey(KeyCode.A) && ControlActive)
        {
            aileronManager.AileronDeflection(1);
        }
        else if (Input.GetKey(KeyCode.D) && ControlActive)
        {
            aileronManager.AileronDeflection(-1);
        }
        else
            aileronManager.AileronDeflection(0);
        /* if (Input.GetKeyDown(KeyCode.A) && ControlActive)
         {
             //leftElleron.Rotate(maxEleronAngle, 0, 0);
            // rightElleron.Rotate(-maxEleronAngle, 0, 0);
         }
         if (Input.GetKeyUp(KeyCode.A) && ControlActive)
         {
             //leftElleron.Rotate(-maxEleronAngle, 0, 0);
             //rightElleron.Rotate(maxEleronAngle, 0, 0);
         }
         if (Input.GetKeyDown(KeyCode.D) && ControlActive)
         {
             //leftElleron.Rotate(-maxEleronAngle, 0, 0);
             //rightElleron.Rotate(maxEleronAngle, 0, 0);
         }
         if (Input.GetKeyUp(KeyCode.D) && ControlActive)
         {
             //leftElleron.Rotate(maxEleronAngle, 0, 0);
            // rightElleron.Rotate(-maxEleronAngle, 0, 0);
         }*/
        /* if (Input.GetKeyDown(KeyCode.Alpha0) && ControlActive)
             Flaps(0);
         if (Input.GetKeyDown(KeyCode.Alpha1) && ControlActive)
             Flaps(1);
         if (Input.GetKeyDown(KeyCode.Alpha2) && ControlActive)
             Flaps(2);
         if (Input.GetKeyDown(KeyCode.Alpha3) && ControlActive)
             Flaps(3);
         if (Input.GetKeyDown(KeyCode.Alpha4) && ControlActive)
             Flaps(4);
         if (Input.GetKeyDown(KeyCode.Alpha5) && ControlActive)
             Flaps(5);
         if (Input.GetKeyDown(KeyCode.Alpha6) && ControlActive)
             Flaps(6);
         if (Input.GetKeyDown(KeyCode.Alpha7) && ControlActive)
             Flaps(7);
         if (Input.GetKeyDown(KeyCode.Alpha8) && ControlActive)
             Flaps(8);*/

        /* if (Input.GetKeyDown(KeyCode.F) && ControlActive)
         {
             flaps += flapsStep;
             if (flaps > 1)
                 flaps = 1;
             leftFlap.GetComponent<WingForce>().degree = flaps;
             rightFlap.GetComponent<WingForce>().degree = flaps;
         }
         if (Input.GetKeyDown(KeyCode.V) && ControlActive)
         {
             Debug.Log("Flaps");
             flaps -= flapsStep;
             if (flaps <0)
                 flaps = 0;
             leftFlap.GetComponent<WingForce>().degree = flaps;
             rightFlap.GetComponent<WingForce>().degree = flaps;
         }*/
        if (Input.GetKeyDown(KeyCode.L) && ControlActive)
        {
            lightManager.On = !lightManager.On;
        }
        if (Input.GetKeyDown(KeyCode.R) && ControlActive)
        {
            Debug.Log("Reverse");
            reverseManager.ReverseToggle();
            spoilersManager.SpoilersToggle();
            /*reverseIsOn = !reverseIsOn;
            if(reverseIsOn)
            {
                leftSpoiler.GetComponent<WingForce>().degree = 1;
                rightSpoiler.GetComponent<WingForce>().degree = 1;
                leftSpoiler.Rotate(90, 0, 0, Space.Self);
                rightSpoiler.Rotate(90, 0, 0, Space.Self);
            }
            else 
            {
                leftSpoiler.GetComponent<WingForce>().degree = 0;
                rightSpoiler.GetComponent<WingForce>().degree = 0;
                leftSpoiler.Rotate(-90, 0, 0, Space.Self);
                rightSpoiler.Rotate(-90, 0, 0, Space.Self);
                for (int i = 0; i < engineLevels.Count; i++)
                    engineLevels[i] = 0.1f;
            }*/

        }
        
        /*if (reverseIsOn)
            for (int i = 0; i < engineLevels.Count; i++)
                engineLevels[i] = engineReverseLevel;*/
        for (int i = 0; i < engineLevels.Count; i++)
            engines[i].SetEngineLevel(engineLevels[i]);

        
    }
    //void FixedUpdate()
    //{


    //    List<Vector3> CurrentForceVectors;
    //    List<Vector3> AbsolutePointsOfForceApplying;
    //    foreach (IForce force in forceSources)
    //    {
    //        force.CountForce(out CurrentForceVectors, out AbsolutePointsOfForceApplying);
    //        for (int i = 0; i < CurrentForceVectors.Count; i++)
    //        {
    //            AddForce(CurrentForceVectors[i], AbsolutePointsOfForceApplying[i]);
    //        }

    //    }
    //    //Debug.Log("Force:" + ForceToCenterOfMass);
    //    rb.AddForce(ForceToCenterOfMass, ForceMode.Force);
    //    MomentInCoordinatesTranslatedToCenterOfMass *= -1;
    //    //Debug.Log("M: " + (MomentInCoordinatesTranslatedToCenterOfMass));
    //    rb.AddRelativeTorque(transform.InverseTransformDirection(MomentInCoordinatesTranslatedToCenterOfMass), ForceMode.Force);
    //    ForceToCenterOfMass = Vector3.zero;
    //    MomentInCoordinatesTranslatedToCenterOfMass = Vector3.zero;
    //}
    public void ToggleGear()
    {
        gearManager.ToggleGears();
    }
    public void Flaps(int pos)
    {
        foreach (FlapsManager m in flapsManagers)
            m.Flaps(pos);
    }
    public void CountState()
    {
        AngularVelocityInLocalCoordinates = transform.InverseTransformDirection(rb.angularVelocity);
        VelocityInLocalCoordinates = transform.InverseTransformDirection(rb.linearVelocity);
    }
    public void Trim(float input)
    {
        float d_angle = trimRate * Time.deltaTime * input;
        Vector3 rot = horizontalStabilizer.localEulerAngles;
        rot.x += d_angle;
        float currentAngle = rot.x;
        if (currentAngle > 180f)
            currentAngle -= 360f;
        if (currentAngle < minTrimAngle)
            rot.x = minTrimAngle;
        if (currentAngle > maxTrimAngle)
            rot.x = maxTrimAngle;
        horizontalStabilizer.localEulerAngles = rot;
    }
    //}
    //private void AddForce(Vector3 forceInWorldCoordinates, Vector3 pointOfApplicationINWorldCoordinates)
    //{
    //    ForceToCenterOfMass += forceInWorldCoordinates;
    //    //Debug.Log("dF" + forceInWorldCoordinates);
    //    Vector3 r = pointOfApplicationINWorldCoordinates - rb.worldCenterOfMass;
    //    //Debug.Log("r" + r);
    //    Vector3 dM = -Vector3.Cross(r, forceInWorldCoordinates);
    //    //Debug.Log("dM: " + dM);
    //    MomentInCoordinatesTranslatedToCenterOfMass += dM;
    //    Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + forceInWorldCoordinates, Color.red);
    //    Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + dM, Color.blue);
    //    Debug.DrawLine(rb.worldCenterOfMass, rb.worldCenterOfMass + r, Color.green);
    //}
}
