using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : ForceCalculationManager
{
    public float generalLevel = 0;
    public float generalLevelChangingSpeed = 1f;
    List<float> engineLevels = new List<float>();
    public float maxEleronAngle = 2;
    public bool ControlActive = true;
    public bool reverseIsOn = false;
    public float engineReverseLevel = -0.2f;
    public float spoilerAngularSpeed = 90f;
    public float spoilerCurrentAngle = 0;
    //public float flapsAngularSpeed = 5f;

    [SerializeField]
    List<EngineForce> engines;
    [SerializeField]
    public GravityForce gravityForce;
    [SerializeField]
    public List<WingForce> wings;
    [SerializeField]
    List<ResistanceForce> resistanceForces;
    [SerializeField]
    Transform heightController;
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

    // Start is called before the first frame update
    void Start()
    {
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
        //forceSources.Add(gravityForce);
        leftSpoiler.GetComponent<WingForce>().degree = 0;
        rightSpoiler.GetComponent<WingForce>().degree = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rb.centerOfMass = centerOfMassLocal;
        Debug.Log("Inertia tensor" + rb.inertiaTensor);
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;
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
        float vwrticalInput = -Input.GetAxis("Mouse Y") * Time.deltaTime * heigtSensitivity;
        if (!ControlActive)
            vwrticalInput = 0;
        //Debug.Log("Input:" + vwrticalInput);
        //Debug.Log("Euler:" + heightController.localEulerAngles.x);
        heightController.Rotate(vwrticalInput, 0, 0, Space.Self);
        heightAngle += vwrticalInput;
        if (heightAngle < -maxHeightAngle)
        {
            heightController.Rotate(-vwrticalInput, 0, 0, Space.Self);
            heightAngle -= vwrticalInput;
        }
        if (heightAngle > maxHeightAngle)
        {
            heightController.Rotate(-vwrticalInput, 0, 0, Space.Self);
            heightAngle -= vwrticalInput;
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
        if (Input.GetKeyDown(KeyCode.A) && ControlActive)
        {
            leftElleron.Rotate(maxEleronAngle, 0, 0);
            rightElleron.Rotate(-maxEleronAngle, 0, 0);
        }
        if (Input.GetKeyUp(KeyCode.A) && ControlActive)
        {
            leftElleron.Rotate(-maxEleronAngle, 0, 0);
            rightElleron.Rotate(maxEleronAngle, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && ControlActive)
        {
            leftElleron.Rotate(-maxEleronAngle, 0, 0);
            rightElleron.Rotate(maxEleronAngle, 0, 0);
        }
        if (Input.GetKeyUp(KeyCode.D) && ControlActive)
        {
            leftElleron.Rotate(maxEleronAngle, 0, 0);
            rightElleron.Rotate(-maxEleronAngle, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.F) && ControlActive)
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
        }
        if (Input.GetKeyDown(KeyCode.R) && ControlActive)
        {
            Debug.Log("Reverse");
            reverseIsOn = !reverseIsOn;
            if (reverseIsOn)
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
            }

        }
        if (Input.GetKeyDown(KeyCode.G) && ControlActive)
        {
            gearsUp = !gearsUp;
            if (gearsUp)
                gearsAudioManager.GearsUp();
            else
                gearsAudioManager.GearsDown();
        }
        if (reverseIsOn)
            for (int i = 0; i < engineLevels.Count; i++)
                engineLevels[i] = engineReverseLevel;
        for (int i = 0; i < engineLevels.Count; i++)
            engines[i].Level = engineLevels[i];
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

    public void CountState()
    {
        AngularVelocityInLocalCoordinates = transform.InverseTransformDirection(rb.angularVelocity);
        VelocityInLocalCoordinates = transform.InverseTransformDirection(rb.velocity);
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
    public IEnumerator SpoilersOn()
    {
        leftSpoiler.Rotate(spoilerAngularSpeed * Time.deltaTime,0,0,Space.Self);
        rightSpoiler.Rotate(spoilerAngularSpeed * Time.deltaTime, 0, 0, Space.Self);
        spoilerCurrentAngle += spoilerAngularSpeed * Time.deltaTime;
        yield return null;
    }
}
