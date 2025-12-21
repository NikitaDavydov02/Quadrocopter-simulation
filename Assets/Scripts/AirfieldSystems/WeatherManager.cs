using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0f, 24f)]
    public float DayTime;
    [SerializeField]
    private float MaxSunAngle = 60f;
    [SerializeField]
    private Light sun;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt_noon = DayTime - 12f;
        float angle_hor = dt_noon * 30f;
        float angle_ver = MaxSunAngle * (1 - Mathf.Abs(dt_noon) / 6f);
        sun.transform.eulerAngles = new Vector3(angle_ver, angle_hor, 0f);
    }
}
