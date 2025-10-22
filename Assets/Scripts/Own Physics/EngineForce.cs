using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineForce : MonoBehaviour, IForce
{

    public Vector3 AxisDirection;
    public float MaxForce;
    private float Level;
    private bool reverseOn;
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
    [SerializeField]
    private float reverseLevel = -0.1f;
    private Vector3 closedPos;
    private Vector3 openedPos;
    [SerializeField]
    private float reverseDoorOpenSpeed =  0.1f;
    [SerializeField]
    private Vector3 reverseDoorOpenOffset;
    [SerializeField]
    private Transform reverseDoor;
    // Start is called before the first frame update
    public void SetEngineLevel(float Level)
    {
        if (!reverseOn)
            this.Level = Level;
    }
    public void ReverseOn()
    {
        reverseOn = true;
        this.Level = reverseLevel;
        StopCoroutine(CloseReverseDoor());
        StartCoroutine(OpenReverseDoor());
    }
    public void ReverseOff()
    {
        reverseOn = false;
        this.Level = 0;
        StopCoroutine(OpenReverseDoor());
        StartCoroutine(CloseReverseDoor());
    }
    IEnumerator OpenReverseDoor()
    {
        //float elapsed = 0f;
        Vector3 dr = openedPos - reverseDoor.localPosition;

        while (Vector3.Dot(dr,reverseDoorOpenOffset)>0)
        {
            //elapsed += Time.deltaTime;
            //float t = Mathf.SmoothStep(0f, 1f, elapsed / reverseDoorOpenDuration);
            reverseDoor.localPosition += reverseDoorOpenOffset.normalized * Time.deltaTime * reverseDoorOpenSpeed;
            dr = openedPos - reverseDoor.localPosition;
            Debug.Log("Open door");
            yield return null;
        }

        reverseDoor.localPosition = openedPos;
    }
    IEnumerator CloseReverseDoor()
    {
        //float elapsed = 0f;
        Vector3 dr = closedPos - reverseDoor.localPosition;

        while (Vector3.Dot(dr, -reverseDoorOpenOffset) > 0)
        {
            //elapsed += Time.deltaTime;
            //float t = Mathf.SmoothStep(0f, 1f, elapsed / reverseDoorOpenDuration);
            reverseDoor.localPosition += (-reverseDoorOpenOffset.normalized) * Time.deltaTime * reverseDoorOpenSpeed;
            dr = closedPos - reverseDoor.localPosition;
            Debug.Log("Close door");
            yield return null;
        }

        reverseDoor.localPosition = closedPos;
    }
    void Start()
    {
        firstRotationForceRelativePoint = new Vector3(AxisRadius, 0, 0);
        secondRotationForceRelativePoint = new Vector3(-AxisRadius, 0, 0);
        closedPos = reverseDoor.localPosition;
        openedPos = closedPos + reverseDoorOpenOffset;
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
