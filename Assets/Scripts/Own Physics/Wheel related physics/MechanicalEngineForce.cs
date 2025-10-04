using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalEngineForce : MonoBehaviour
{
    public Vector3 AxisDirection;
    public float MaxMomentum;
    public float Level;

    [SerializeField]
    List<WheelForce> wheels;
    [SerializeField]
    private float gasIncrementSpeed;

    [SerializeField]
    private EngineAudioManager engineAudioManager;
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if (engineAudioManager != null)
            engineAudioManager.level = Level;
        if (Input.GetKey(KeyCode.W))
            Level += gasIncrementSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            Level -= gasIncrementSpeed * Time.deltaTime;
        if (Level > 1)
            Level = 1;
        if (Level < 0)
            Level = 0;
        Vector3 engineAxisGlobal = transform.TransformDirection(AxisDirection);
        foreach(WheelForce wheel in wheels)
        {
            wheel.globalAxisMomentum = engineAxisGlobal * MaxMomentum * Level;
        }
    }

  
}
