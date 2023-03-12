using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForceCalculationManager : MonoBehaviour
{
    protected List<IForce> forceSources;
    public Vector3 ForceToCenterOfMass { get; protected set; }
    protected Vector3 MomentInCoordinatesTranslatedToCenterOfMass;
    public Rigidbody rb { get; protected set; }
    // Start is called before the first frame update
    protected void Init()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        forceSources = new List<IForce>();
    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        ////Physics counting
        //Physics counting
        

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
        ////Debug.Log("Force:" + ForceToCenterOfMass);
        //rb.AddForce(ForceToCenterOfMass, ForceMode.Force);
        ////Debug.Log("M: " + MomentInCoordinatesTranslatedToCenterOfMass);
        //rb.AddTorque(MomentInCoordinatesTranslatedToCenterOfMass, ForceMode.Force);
        //ForceToCenterOfMass = Vector3.zero;
        //MomentInCoordinatesTranslatedToCenterOfMass = Vector3.zero;
        //return;
        rb.AddForce(ForceToCenterOfMass, ForceMode.Force);
        MomentInCoordinatesTranslatedToCenterOfMass *= -1;
        //Debug.Log("M: " + (MomentInCoordinatesTranslatedToCenterOfMass));
        rb.AddRelativeTorque(transform.InverseTransformDirection(MomentInCoordinatesTranslatedToCenterOfMass), ForceMode.Force);
        ForceToCenterOfMass = Vector3.zero;
        MomentInCoordinatesTranslatedToCenterOfMass = Vector3.zero;
        return;

    }
    public void AddForce(Vector3 forceInWorldCoordinates, Vector3 pointOfApplicationINWorldCoordinates)
    {
        //ForceToCenterOfMass += forceInWorldCoordinates;
        ////Debug.Log("forceInWorldCoordinates" + forceInWorldCoordinates);
        //Vector3 r = pointOfApplicationINWorldCoordinates - transform.position;
        ////Vector3 r = pointOfApplicationINWorldCoordinates;
        //Vector3 dM = -Vector3.Cross(r, forceInWorldCoordinates);
        ////Debug.Log("dM: " + dM);
        //MomentInCoordinatesTranslatedToCenterOfMass += dM;
        //Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + forceInWorldCoordinates, Color.red);
        //Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + dM, Color.blue);
        ForceToCenterOfMass += forceInWorldCoordinates;
        //Debug.Log("dF" + forceInWorldCoordinates);
        Vector3 r = pointOfApplicationINWorldCoordinates - rb.worldCenterOfMass;
        //Debug.Log("r" + r);
        Vector3 dM = -Vector3.Cross(r, forceInWorldCoordinates);
        //Debug.Log("dM: " + dM);
        MomentInCoordinatesTranslatedToCenterOfMass += dM;
        Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + forceInWorldCoordinates, Color.red);
        Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + dM, Color.blue);
        Debug.DrawLine(rb.worldCenterOfMass, rb.worldCenterOfMass + r, Color.green);
    }
}
