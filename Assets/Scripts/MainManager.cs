using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Rigidbody rb;
    public static float AirDensity = 1.05f;
    public static float Temperature = 293f;
    [SerializeField]
    public static Vector3 WindVector = new Vector3(0, 0, 0);
    [SerializeField]
    public float amplitude = 0;
    public float XAmplitude = 5000;
    public float YAmplitude = 5000;
    public float XGridStep = 1;
    void Start()
    {
        WindVector = Vector3.forward * amplitude;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static Vector3 GetWind(Vector3 posotion)
    {
        //return Vector3.zero;
        //Vector3 eddy = new Vector3(Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude));
        //seddy = Vector3.zero;
        return WindVector;
    }
}
