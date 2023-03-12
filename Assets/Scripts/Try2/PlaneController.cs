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


    public Vector3 VelocityInLocalCoordinates = Vector3.zero;
    public Vector3 AngularVelocityInLocalCoordinates = Vector3.zero;
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
    }

    // Update is called once per frame
    void Update()
    {
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
        if (heightAngle < -10)
        {
            heightController.Rotate(-vwrticalInput, 0, 0, Space.Self);
            heightAngle -= vwrticalInput;
        }
        if (heightAngle > 10)
        {
            heightController.Rotate(-vwrticalInput, 0, 0, Space.Self);
            heightAngle -= vwrticalInput;
        }
        float horInput = -Input.GetAxis("Mouse X") * Time.deltaTime * horizontalSensitivity;
        if (!ControlActive)
            horInput = 0;
        horizontalController.Rotate(horInput, 0, 0, Space.Self);
        horizontAngle += horInput;
        if (horizontAngle < -10 || horizontAngle > 10)
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
}
