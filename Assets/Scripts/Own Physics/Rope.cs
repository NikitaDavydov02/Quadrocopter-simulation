using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope:MonoBehaviour
{
    [SerializeField]
    ForceCalculationManager first;
    [SerializeField]
    ForceCalculationManager second;
    public float maxLength = 2f;
    public float k = 1000;
    public Vector3 relativeFirstPosition;
    public Vector3 relativeSecondPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 firstPoint =  first.transform.position + first.transform.TransformDirection(relativeFirstPosition);
        Vector3 secondPoint = second.transform.position + second.transform.TransformDirection(relativeSecondPosition);
        if (Vector3.Magnitude(secondPoint - firstPoint) > maxLength)
        {
            Vector3 secondActsOnFirstDirection = Vector3.Normalize(second.transform.position - first.transform.position);
            Vector3 forseActingOnFirst = first.ForceToCenterOfMass;
            Vector3 forceActingOnSecond = second.ForceToCenterOfMass;

            float v2ProjectedToRope = Vector3.Dot(second.rb.velocity, secondActsOnFirstDirection);
            float v1ProjectedToRope = Vector3.Dot(first.rb.velocity, secondActsOnFirstDirection);
            float delta = Vector3.Magnitude(secondPoint - firstPoint) - maxLength;
            Vector3 forseToFirst = secondActsOnFirstDirection * delta * k;
            Vector3 forseToSecond = -forseToFirst;
            first.AddForce(forseToFirst,firstPoint);
            second.AddForce(forseToSecond, secondPoint);

        }
    }

}
