using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentForce : MonoBehaviour
{
    public Vector3 CurrentForceVector { get; private set; }
    public Vector3 RelativeToCenterPointOfForceApplying { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        RelativeToCenterPointOfForceApplying = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void AddForceToCurrentForce(Vector3 additionalForce, Vector3 additionalForcePoint)
    {
        CurrentForceVector += additionalForce;
    }
}
