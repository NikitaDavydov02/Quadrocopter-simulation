using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AileronManager : MonoBehaviour
{
    [SerializeField]
    private Transform leftAileron;
    [SerializeField]
    private Transform rightAileron;
    public float maxAileronAngle;
    public float aileronRotationSpeed;
    // Start is called before the first frame update
    private float input = 0;
    Quaternion leftInit;
    Quaternion rightInit;
    private float currentAngle;
    void Start()
    {
        leftInit = leftAileron.localRotation;
        rightInit = rightAileron.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (input != 0)
        {
            currentAngle += input * aileronRotationSpeed * Time.deltaTime;
        }
        else
        {
            if(currentAngle>0)
                currentAngle -= aileronRotationSpeed * Time.deltaTime;
            else
                currentAngle += aileronRotationSpeed * Time.deltaTime;
        }
        currentAngle = Mathf.Clamp(currentAngle, -maxAileronAngle, maxAileronAngle);
        Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        leftAileron.localRotation = leftInit * rotation;
        rightAileron.localRotation = rightInit * rotation;
    }
    public void AileronDeflection(float input)
    {
        this.input = input;
    }
}
