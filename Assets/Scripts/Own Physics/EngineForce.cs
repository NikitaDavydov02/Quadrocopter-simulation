using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineForce : MonoBehaviour, IForce
{

    public Vector3 AxisDirection;
    public float MaxForce;
    public float Level;
    public float AxisRadius = 0.1f;
    public float RotationCoeffitient = 1f;
    public bool jet = false;

    Vector3 firstRotationForceRelativePoint;
    Vector3 secondRotationForceRelativePoint;
    Vector3 firstRotationForceRelative;
    Vector3 secondRotationForceRelative;

    public bool clockwiseRotation;

    [SerializeField]
    private EngineAudioManager engineAudioManager;
    // Start is called before the first frame update
    void Start()
    {
        firstRotationForceRelativePoint = new Vector3(AxisRadius, 0, 0);
        secondRotationForceRelativePoint = new Vector3(-AxisRadius, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
       if(engineAudioManager!=null)
            engineAudioManager.level = Level;
    }

    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>();
        AbsolutePointsOfForceApplying = new List<Vector3>();
        Vector3 force = AxisDirection * MaxForce * Level;
        
        force = transform.TransformDirection(force);
        CurrentForceVectors.Add(force);
        Vector3 pointOfApplication = Vector3.zero;
        AbsolutePointsOfForceApplying.Add(transform.TransformPoint(pointOfApplication));
        Vector3 firstRotationForceRelative = RotationCoeffitient*MaxForce * Level * new Vector3(0, 0, 1);
        Vector3 secondRotationForceRelative = -firstRotationForceRelative;
        if (!clockwiseRotation)
        {
            firstRotationForceRelative *= -1;
            secondRotationForceRelative *= -1;
        }
        if (jet)
            return;
        CurrentForceVectors.Add(transform.TransformDirection(firstRotationForceRelative));
        CurrentForceVectors.Add(transform.TransformDirection(secondRotationForceRelative));
        AbsolutePointsOfForceApplying.Add(transform.TransformPoint(firstRotationForceRelativePoint));
        AbsolutePointsOfForceApplying.Add(transform.TransformPoint(secondRotationForceRelativePoint));
    }
}
