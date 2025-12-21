using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    // Start is called before the first frame update
    //[SerializeField]
    //Rigidbody rb;
    public  float AirDensity = 1.05f;
    public  float Temperature = 293f;
    //[SerializeField]
    private Vector3 WindVector = new Vector3(0, 0, 0);
    [SerializeField]
    public float WindAmplitude = 0;
    [SerializeField]
    [Range(0f, 360f)]
    public float WindAzimuth = 0f;
    /*public float XAmplitude = 5000;
        public float YAmplitude = 5000;
        public float XGridStep = 1;*/
    [Range(0f, 24f)]
    public float DayTime;
    [SerializeField]
    private float MaxSunAngle = 60f;
    [SerializeField]
    private Light sun;

    private void Awake()
    {
        // Ensure there is only one instance
        if (Instance == null)
        {
            Instance = this;
            // Optional: Keeps it alive across different scenes
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
    void Start()
    {
        //WindVector = Vector3.forward * amplitude;
    }

    // Update is called once per frame
    void Update()
    {
        /* float amp = 40f;
         Vector3 delta = Vector3.zero;
         delta.x = Random.Range(-amp, amp);
         delta.y = Random.Range(-amp, amp);
         delta.z = Random.Range(-amp, amp);
         WindVector = delta ;
         Debug.Log("Wind: " + WindVector);*/
        float dt_noon = DayTime - 12f;
        float angle_hor = dt_noon * 30f;
        float angle_ver = MaxSunAngle * (1 - Mathf.Abs(dt_noon) / 6f);
        sun.transform.eulerAngles = new Vector3(angle_ver, angle_hor, 0f);
    }
    public Vector3 GetWind(Vector3 posotion)
    {
        //return Vector3.zero;
        //Vector3 eddy = new Vector3(Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude));
        //seddy = Vector3.zero;
        WindVector = new Vector3(0, 0, 0);
        WindVector.x = WindAmplitude * Mathf.Sin(WindAzimuth * Mathf.Deg2Rad);
        WindVector.z = WindAmplitude * Mathf.Cos(WindAzimuth * Mathf.Deg2Rad);
        WindVector = -WindVector;
        return WindVector;
    }
    public float GetAirDensity(float altitude)
    {
        //return Vector3.zero;
        //Vector3 eddy = new Vector3(Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude));
        //seddy = Vector3.zero;
        return AirDensity*Mathf.Exp(-0.029f*9.81f*altitude/(8.314f*273));
    }
}
