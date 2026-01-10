using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoilersManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private List<Transform> leftSpoilersTransform;
    [SerializeField]
    private List<Transform> rightSpoilersTransform;
    
    //[SerializeField]
    private List<Vector3> leftRetractedLocalRotation;
    private List<Vector3> rightRetractedLocalRotation;
    private List<Vector3> leftExtandedLocalRotation;
    private List<Vector3> rightExtandedLocalRotation;
    int spoilersLeftCount;
    int spoilersRightCount;
    public float openAngle;
    private bool spoilersExtended;
    public float spoilerRotationSpeed;
    void Start()
    {
        spoilersExtended = false;
        spoilersLeftCount = leftSpoilersTransform.Count;
        spoilersRightCount = rightSpoilersTransform.Count;

        leftRetractedLocalRotation = new List<Vector3>();
        rightRetractedLocalRotation = new List<Vector3>();
        leftExtandedLocalRotation = new List<Vector3>();
        rightExtandedLocalRotation = new List<Vector3>();

        for (int i = 0; i < spoilersLeftCount; i++)
        {
            leftRetractedLocalRotation.Add(leftSpoilersTransform[i].localEulerAngles);
            rightRetractedLocalRotation.Add(rightSpoilersTransform[i].localEulerAngles);

            Vector3 leftRot = leftRetractedLocalRotation[i];
            Vector3 rightRot = rightRetractedLocalRotation[i];

            leftRot.z += openAngle;
            rightRot.z += openAngle;

            leftExtandedLocalRotation.Add(leftRot);
            rightExtandedLocalRotation.Add(rightRot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ExtendSpoilersAnimation()
    {
        Vector3[] d_left_angle = new Vector3[spoilersLeftCount];
        Vector3[] d_right_angle = new Vector3[spoilersLeftCount];

        bool stopExtending = false;
        
        for(int i = 0; i < spoilersLeftCount; i++)
        {
            d_left_angle[i] = leftExtandedLocalRotation[i] - leftSpoilersTransform[i].localEulerAngles;
            d_right_angle[i] = rightExtandedLocalRotation[i] - rightSpoilersTransform[i].localEulerAngles;
        }
        //Debug.Log("Extend");
        while (!stopExtending)
        {
            //Debug.Log("Extending");
            stopExtending = true;
            for (int i = 0; i < spoilersLeftCount; i++)
            {
                if (d_left_angle[i].z < 0)
                {
                    leftSpoilersTransform[i].Rotate(Vector3.forward, spoilerRotationSpeed * Time.deltaTime);
                    d_left_angle[i] = leftExtandedLocalRotation[i] - leftSpoilersTransform[i].localEulerAngles;
                    stopExtending = false;
                }
                if (d_right_angle[i].z < 0)
                {
                    rightSpoilersTransform[i].Rotate(Vector3.forward, spoilerRotationSpeed * Time.deltaTime);
                    d_right_angle[i] = rightExtandedLocalRotation[i] - rightSpoilersTransform[i].localEulerAngles;
                    stopExtending = false;
                }
            }
            yield return null;
        }
        //Debug.Log("Extend end");

        for (int i = 0; i < spoilersLeftCount; i++)
        {
            leftSpoilersTransform[i].localEulerAngles = leftExtandedLocalRotation[i];
            rightSpoilersTransform[i].localEulerAngles = rightExtandedLocalRotation[i];
        }
    }
    IEnumerator RetractSpoilersAnimation()
    {
        Vector3[] d_left_angle = new Vector3[spoilersLeftCount];
        Vector3[] d_right_angle = new Vector3[spoilersLeftCount];

        bool stopExtending = false;

        for (int i = 0; i < spoilersLeftCount; i++)
        {
            d_left_angle[i] = leftRetractedLocalRotation[i] - leftSpoilersTransform[i].localEulerAngles;
            d_right_angle[i] = rightRetractedLocalRotation[i] - rightSpoilersTransform[i].localEulerAngles;
        }
        //Debug.Log("Extend");
        while (!stopExtending)
        {
            //Debug.Log("Extending");
            stopExtending = true;
            for (int i = 0; i < spoilersLeftCount; i++)
            {
                if (d_left_angle[i].z > 0)
                {
                    leftSpoilersTransform[i].Rotate(Vector3.forward, -spoilerRotationSpeed * Time.deltaTime);
                    d_left_angle[i] = leftRetractedLocalRotation[i] - leftSpoilersTransform[i].localEulerAngles;
                    stopExtending = false;
                }
                if (d_right_angle[i].z > 0)
                {
                    rightSpoilersTransform[i].Rotate(Vector3.forward, -spoilerRotationSpeed * Time.deltaTime);
                    d_right_angle[i] = rightRetractedLocalRotation[i] - rightSpoilersTransform[i].localEulerAngles;
                    stopExtending = false;
                }
            }
            yield return null;
        }
        //Debug.Log("Extend end");

        for (int i = 0; i < spoilersLeftCount; i++)
        {
            leftSpoilersTransform[i].localEulerAngles = leftRetractedLocalRotation[i];
            rightSpoilersTransform[i].localEulerAngles = rightRetractedLocalRotation[i];
        }
    }
    public void SpoilersToggle()
    {
        StopAllCoroutines();
        spoilersExtended = !spoilersExtended;
        if (spoilersExtended)
        {
            StartCoroutine(ExtendSpoilersAnimation());
        }
        else
        {
            StartCoroutine(RetractSpoilersAnimation());
        }
    }
}
