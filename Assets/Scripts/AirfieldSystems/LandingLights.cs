using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingLights : MonoBehaviour
{
    [SerializeField]
    private Material red;
    [SerializeField]
    private Material white;
    [SerializeField]
    private MeshRenderer[] landingLightObjects;
    [SerializeField]
    private Light[] landingLights;

    private GameObject aircraft;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] aircrafts = GameObject.FindGameObjectsWithTag("Aircraft");
        if (aircrafts.Length != 0)
            aircraft = aircrafts[0];
        for (int i = 0; i < 4; i++)
        {
            landingLights[i].color = Color.red;
            landingLightObjects[i].material = red;
        }
        if (aircraft != null)
        {
            Vector3 absDirectionToTheAirplane = aircraft.transform.position - transform.position;
            Vector3 relDirectionToTheAirplane = transform.InverseTransformDirection(absDirectionToTheAirplane);
            float angle = Vector3.Angle(Vector3.forward, relDirectionToTheAirplane);
            if (Mathf.Abs(angle) < 40f)
            {
                relDirectionToTheAirplane.x = 0;
                float ver_angle = Vector3.Angle(Vector3.forward, relDirectionToTheAirplane);
                int num_white = 0;
                if (ver_angle > 2.5f)
                    num_white++;
                if (ver_angle > 2.87f)
                    num_white++;
                if (ver_angle > 3.17f)
                    num_white++;
                if (ver_angle > 3.5f)
                    num_white++;
                for(int i = 0; i < num_white; i++)
                {
                    landingLights[i].color = Color.white;
                    landingLightObjects[i].material = white;
                }
            }
        }
    }
}
