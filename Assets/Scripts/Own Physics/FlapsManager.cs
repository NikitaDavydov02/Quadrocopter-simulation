using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapsManager : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    [SerializeField]
    private List<Transform> leftFlapsTransform;
    [SerializeField]
    private List<Transform> rightFlapsTransform;
    //[SerializeField]
    Quaternion[] left_flap_init;
    Quaternion[] right_flap_init;
    Quaternion[] left_flap_extended;
    Quaternion[] right_flap_extended;
    
    int FlapsLeftCount;
    int FlapsRightCount;

    public float stageTime;
    [SerializeField]
    private List<float> angles;
    [SerializeField]
    private List<Vector3> offsets;
    int currentPos;

    [SerializeField]
    private List<WingForce> flaps;

    [SerializeField]
    FlapsAudioManager flapsAudioManager;
    void Start()
    {
        currentPos = 0;
        FlapsLeftCount = leftFlapsTransform.Count;
        FlapsRightCount = rightFlapsTransform.Count;

        left_flap_init = new Quaternion[FlapsLeftCount];
        right_flap_init = new Quaternion[FlapsLeftCount];

        left_flap_extended = new Quaternion[FlapsLeftCount];
        right_flap_extended = new Quaternion[FlapsLeftCount];

        if (flaps != null)
        {
            foreach (WingForce w in flaps)
                w.degree = 0;
        }

        /* leftRetractedLocalPosition = new List<Vector3>();
         rightRetractedLocalPosition = new List<Vector3>();

         for (int i = 0; i < FlapsLeftCount; i++)
         {
             leftRetractedLocalPosition.Add(leftFlapsTransform[i].transform.localPosition);
             rightRetractedLocalPosition.Add(rightFlapsTransform[i].transform.localPosition);
         }*/
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator ExtendFlapsAnimation(int nextPositionIndex)
    {
        Debug.Log(gameObject.name + " extend to position " + nextPositionIndex);
        Vector3[] left_start_pos = new Vector3[FlapsLeftCount];
        Vector3[] right_start_pos = new Vector3[FlapsLeftCount];

        float d_angle = angles[nextPositionIndex] - angles[currentPos];
        
        Quaternion rotation = Quaternion.AngleAxis(d_angle, Vector3.forward);

        for (int i = 0; i < FlapsLeftCount; i++)
        {
            //d_left_angle[i] = leftExtandedLocalRotation[i] - leftFlapsTransform[i].localEulerAngles;
            //d_right_angle[i] = rightExtandedLocalRotation[i] - rightFlapsTransform[i].localEulerAngles;
            left_flap_init[i] = leftFlapsTransform[i].localRotation;
            right_flap_init[i] = rightFlapsTransform[i].localRotation;
            left_flap_extended[i] = left_flap_init[i] * rotation;
            right_flap_extended[i] = right_flap_init[i] * rotation;
            left_start_pos[i] = leftFlapsTransform[i].localPosition;
            right_start_pos[i] = rightFlapsTransform[i].localPosition;
        }
        Debug.Log("Extend flaps start local pos: " + leftFlapsTransform[0].localPosition);
        //Debug.Log("Extend flaps start");
        //Debug.Log("d_left_angle flaps: " + d_left_angle[0]);
        //Debug.Log("d_right_angle flaps: " + d_right_angle[0]);
        float extensionTime = stageTime;
        float elapsedTime = 0;
        while (elapsedTime < extensionTime)
        {
            Debug.Log("Extending flaps: " + elapsedTime / extensionTime);
            for (int i = 0; i < FlapsLeftCount; i++)
            {
                //Quaternion leftCurrent = leftFlapsTransform[i].rotation;
                // Quaternion rotation = Quaternion.AngleAxis(FlapRotationSpeed * Time.deltaTime, Vector3.forward);
                //leftFlapsTransform[i].rotation = leftFlapsTransform[i].rotation * rotation;
                Quaternion left = Quaternion.Slerp(left_flap_init[i], left_flap_extended[i], elapsedTime / extensionTime);
                Quaternion right = Quaternion.Slerp(right_flap_init[i], right_flap_extended[i], elapsedTime / extensionTime);
                leftFlapsTransform[i].localRotation = left;
                rightFlapsTransform[i].localRotation = right;

                Vector3 left_pos = Vector3.Lerp(left_start_pos[i], left_start_pos[i]+offsets[nextPositionIndex]- offsets[currentPos], elapsedTime / extensionTime);
                Vector3 right_pos = Vector3.Lerp(right_start_pos[i], right_start_pos[i]+offsets[nextPositionIndex]-offsets[currentPos], elapsedTime / extensionTime);
                leftFlapsTransform[i].localPosition = left_pos;
                rightFlapsTransform[i].localPosition =right_pos;
                Debug.Log(gameObject.name + " flaps pos: " + left_pos);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentPos = nextPositionIndex;
        if (flaps != null)
        {
            float degree = currentPos / (float)(offsets.Count-1);
            foreach (WingForce w in flaps)
                w.degree = degree;
            Debug.Log("Set degree: " + degree);
        }
    }
    public void Flaps(int pos)
    {
        if (pos == currentPos)
            return;
        
        StopAllCoroutines();
        StartCoroutine(SequentialFlapsExtension(pos));
    }
    private IEnumerator SequentialFlapsExtension(int pos)
    {
        Debug.Log(gameObject.name + " start sequential");
        int step = 0;
        if (pos > currentPos)
            step = 1;
        else
            step = -1;
        if(flapsAudioManager!=null)
            flapsAudioManager.StartFlaps();
        for (int i = currentPos; i != pos; i += step)
        {
            yield return StartCoroutine(ExtendFlapsAnimation(i + step));

        }
        if (flapsAudioManager != null)
            flapsAudioManager.StopFlaps();
    }
}
