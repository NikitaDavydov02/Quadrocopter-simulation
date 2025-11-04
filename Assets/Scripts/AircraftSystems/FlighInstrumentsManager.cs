using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlighInstrumentsManager : MonoBehaviour
{
    [SerializeField]
    private Transform gyroscope;
    [SerializeField]
    private EngineForce leftEngine;
    [SerializeField]
    private EngineForce rightEngine;
    public float PitchAngle { get
        {
            return gyroscope.transform.eulerAngles.x;
        } }
    public float BankAngle
    {
        get
        {
            return gyroscope.transform.eulerAngles.z;
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
