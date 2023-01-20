using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightManager : MonoBehaviour
{
    [SerializeField]
    public List<ForceScript> engines;
    // Start is called before the first frame update
    public float generalLevel = 0;
    public float generalLevelChangingSpeed = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
            generalLevel += generalLevelChangingSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.PageDown))
            generalLevel -= generalLevelChangingSpeed * Time.deltaTime;
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < 0)
            generalLevel = 0;
        foreach (ForceScript engine in engines)
            engine.level = generalLevel;
    }
}
