using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

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

   // [SerializeField] VolumeProfile profile;
    [SerializeField]
    public Volume globalVolume;
    [SerializeField]
    public VolumetricClouds clouds;
    private CloudMode cloudState;
    private float cloudheight;

    [SerializeField]
    private PlaneController plane;

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
        //VolumeProfile profile = volumeProfile.GetComponent<Volume>().sharedProfile;

        if (globalVolume.profile.TryGet<VolumetricClouds>(out clouds))
        {
            clouds.active = true;
            Debug.Log("Clouds!");
            //clouds.densityMultiplier.value = 0.8f;
        }
        else
            Debug.Log("No Clouds :(");

  

        //profile.TryGet<VolumetricClouds>(out clouds);
        //clouds.cloudPreset.value = VolumetricClouds.CloudPresets.Overcast;
        //clouds.cloudPreset = VolumetricClouds.CloudPresets.Overcast;
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

        if (angle_ver < -11f)
        {
            //Night
            sun.intensity = 0.25f;
            sun.colorTemperature = 20000f;
            sun.transform.eulerAngles = new Vector3(20f, 180-angle_hor, 0f);
            HDAdditionalLightData hdLight = sun.GetComponent<HDAdditionalLightData>();
            hdLight.flareSize = 0f;
        }
        else
        {
            sun.intensity = 100000f;
            sun.colorTemperature = 6633;
            HDAdditionalLightData hdLight = sun.GetComponent<HDAdditionalLightData>();
            hdLight.flareSize = 2f;
        }

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
    public void SetCloudHeight(float heightMeters)
    {
        clouds.bottomAltitude.value = heightMeters;
        cloudheight = heightMeters;
    }
    public void OnSubsceneEnable()
    {
        if(cloudState!=CloudMode.None)
            clouds.active = true;
    }
    public void OnSubsceneDisnable()
    {
        clouds.active = false;
    }
    public void ChangeCloudType(CloudMode mode)
    {
        cloudState = mode;
        clouds.active = true;
        if (mode == CloudMode.Overcast)
            clouds.cloudPreset = VolumetricClouds.CloudPresets.Overcast;
        if (mode == CloudMode.Sparse)
            clouds.cloudPreset = VolumetricClouds.CloudPresets.Sparse;
        if (mode == CloudMode.None)
            clouds.active = false;
        clouds.bottomAltitude.value = cloudheight;
    }
}
public enum CloudMode
{
    None,
    Sparse,
    Overcast,
}