using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftLightManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private List<LightBulb> navigationalLights;
    [SerializeField]
    private List<LightBulb> strobeLights;
    [SerializeField]
    private List<LightBulb> landingLights;
    [SerializeField]
    private List<LightBulb> taxiLights;
    [SerializeField]
    private float navigationalLightIntensity = 1f;
    [SerializeField]
    private float strobeLightIntensity = 1f;
    [SerializeField]
    private float landingLightIntensity = 1f;
    [SerializeField]
    private float taxiLightIntensity = 1f;
    [SerializeField]
    private float strobeFrequency = 1f;
    [SerializeField]
    private float strobeProportion = 0.5f;
    private float time = 0f;

    public bool On = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!On)
        {
            foreach (LightBulb bulb in navigationalLights)
                bulb.intensity = 0;
            foreach (LightBulb bulb in landingLights)
                bulb.intensity = 0;
            foreach (LightBulb bulb in taxiLights)
                bulb.intensity = 0;
            foreach (LightBulb strobeBulb in strobeLights)
                strobeBulb.intensity = 0;
            return;
        }
        time += Time.deltaTime;
        foreach (LightBulb bulb in navigationalLights)
            bulb.intensity = navigationalLightIntensity;
        foreach (LightBulb bulb in landingLights)
            bulb.intensity = landingLightIntensity;
        foreach (LightBulb bulb in taxiLights)
            bulb.intensity = taxiLightIntensity;
        double period = 1 / strobeFrequency;
        float effectiveStrobeLightIntensity = strobeLightIntensity;
        if (time > strobeProportion * period)
            effectiveStrobeLightIntensity = 0;
        foreach (LightBulb strobeBulb in strobeLights)
            strobeBulb.intensity = effectiveStrobeLightIntensity;
        if (time > period)
            time = 0;
    }
}
