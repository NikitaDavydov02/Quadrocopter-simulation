using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseManager : MonoBehaviour
{
    private bool reverseOn=false;
    [SerializeField]
    private List<EngineForce> engines;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReverseToggle()
    {
        reverseOn = !reverseOn;
        if (reverseOn)
        {
            foreach (EngineForce e in engines)
                e.ReverseOn();
        }
        else
        {
            foreach (EngineForce e in engines)
                e.ReverseOff();
        }
        
    }
}
