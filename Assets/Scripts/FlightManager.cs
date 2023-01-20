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
    public float rotationSensitivity = 1f;
    public float MaxMoment = 100f;
    List<float> engineLevels = new List<float>();
    Rigidbody rb;
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        generalLevel = 0;
        for(int i = 0; i < engines.Count; i++)
        {
            engineLevels.Add(0);
            engines[0].level = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ///General level
        if (Input.GetKey(KeyCode.PageUp))
            generalLevel += generalLevelChangingSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.PageDown))
            generalLevel -= generalLevelChangingSpeed * Time.deltaTime;
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < 0)
            generalLevel = 0;
        for (int i = 0; i < engineLevels.Count; i++)
            engineLevels[i] = generalLevel;
        //Rotations
        float rotationInput = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
        Debug.Log("Rotation: " + rotationInput);
        Vector3 moment = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //rb.AddRelativeTorque(new Vector3(0, MaxMoment, 0));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //rb.AddRelativeTorque(new Vector3(0, MaxMoment, 0));
        }
        ///---------------------------------------///
        //for (int i = 0; i < engines.Count; i++)
            //engines[i].level = engineLevels[i];
    }
}
