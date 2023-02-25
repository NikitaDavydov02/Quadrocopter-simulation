using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrocopterFightController : MonoBehaviour
{
    // Start is called before the first frame update
    public float generalLevel = 0;
    public float generalLevelChangingSpeed = 1f;
    List<float> engineLevels = new List<float>();

    public List<List<float>> InertiaTensor;

    private List<List<float>> invertedInertiaTensor;

    [SerializeField]
    List<EngineForce> engines;
    [SerializeField]
    public GravityForce gravityForce;
    [SerializeField]
    List<ResistanceForce> resistanceForces;

    private Vector3 ForceToCenterOfMass;
    private Vector3 MomentInCoordinatesTranslatedToCenterOfMass;

    private Vector3 velocityOfCenterMass = Vector3.zero;
    private Vector3 AbsoluteAngularVelocity = Vector3.zero;
    private Vector3 LInCoordinatesTranslatedToCenterOfMass = Vector3.zero;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private List<IForce> forceSources;


    public float RotationPowerMultiplyer;
    public float gasSensitivity;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.inertiaTensor = new Vector3(1, 1, 1);
        forceSources = new List<IForce>();
        //gravityForce = this.gameObject.GetComponent<GravityForce>();
        lastPosition = transform.position;
        generalLevel = 0;
        for (int i = 0; i < engines.Count; i++)
        {
            engineLevels.Add(0);
            forceSources.Add(engines[i]);
        }
        foreach (ResistanceForce f in resistanceForces)
            forceSources.Add(f);
        forceSources.Add(gravityForce);
    }

    // Update is called once per frame
    void Update()
    {
        float gas = Input.GetAxis("Mouse ScrollWheel") * gasSensitivity * Time.deltaTime;
        //generalLevel += gas;
        if (Input.GetKey(KeyCode.PageUp))
            generalLevel += generalLevelChangingSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.PageDown))
            generalLevel -= generalLevelChangingSpeed * Time.deltaTime;
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < 0)
            generalLevel = 0;
        for (int i = 0; i < engineLevels.Count; i++)
            engineLevels[i] = generalLevel;
        if (Input.GetKey(KeyCode.S))
        {
            engineLevels[0] = generalLevel*RotationPowerMultiplyer;
            engineLevels[1] = generalLevel * RotationPowerMultiplyer;
        }
        if (Input.GetKey(KeyCode.W))
        {
            engineLevels[2] = generalLevel * RotationPowerMultiplyer;
            engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        }
        if (Input.GetKey(KeyCode.D))
        {
            engineLevels[0] = generalLevel * RotationPowerMultiplyer;
            engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        }
        if (Input.GetKey(KeyCode.A))
        {
            engineLevels[1] = generalLevel * RotationPowerMultiplyer;
            engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        }
        if (Input.GetKey(KeyCode.E))
        {
            engineLevels[1] = generalLevel * RotationPowerMultiplyer;
            engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            engineLevels[0] = generalLevel * RotationPowerMultiplyer;
            engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        }
        for (int i = 0; i < engineLevels.Count; i++)
            engines[i].Level = engineLevels[i];

        
    }
    void FixedUpdate()
    {
        //Physics counting
        ForceToCenterOfMass = Vector3.zero;
        MomentInCoordinatesTranslatedToCenterOfMass = Vector3.zero;

        List<Vector3> CurrentForceVectors;
        List<Vector3> AbsolutePointsOfForceApplying;
        foreach (IForce force in forceSources)
        {
            force.CountForce(out CurrentForceVectors, out AbsolutePointsOfForceApplying);
            for (int i = 0; i < CurrentForceVectors.Count; i++)
            {
                AddForce(CurrentForceVectors[i], AbsolutePointsOfForceApplying[i]);
                Debug.DrawLine(AbsolutePointsOfForceApplying[i], AbsolutePointsOfForceApplying[i] + CurrentForceVectors[i], Color.red);
            }

        }
        Debug.Log("Force:" + ForceToCenterOfMass);
        rb.AddForce(ForceToCenterOfMass, ForceMode.Force);
        Debug.Log("M: " + MomentInCoordinatesTranslatedToCenterOfMass);
        rb.AddTorque(MomentInCoordinatesTranslatedToCenterOfMass, ForceMode.Force);
        return;

    }
    private void AddForce(Vector3 forceInWorldCoordinates, Vector3 pointOfApplicationINWorldCoordinates)
    {
        ForceToCenterOfMass += forceInWorldCoordinates;
        Debug.Log("forceInWorldCoordinates" + forceInWorldCoordinates);
        Vector3 r = pointOfApplicationINWorldCoordinates - transform.position;
        //Vector3 r = pointOfApplicationINWorldCoordinates;
        Vector3 dM = -Vector3.Cross(r, forceInWorldCoordinates);
        Debug.Log("dM: " + dM);
        MomentInCoordinatesTranslatedToCenterOfMass += dM;
        //Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + forceInWorldCoordinates, Color.blue);
    }
}
