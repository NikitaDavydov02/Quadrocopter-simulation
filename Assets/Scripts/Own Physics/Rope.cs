using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    Rigidbody first;
    [SerializeField]
    Rigidbody second;
    public float maxLength = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(second.transform.position - first.transform.position) > maxLength)
        {
            Vector3 secondActsOnFirstDirection = Vector3.Normalize(second.transform.position - first.transform.position);
            Vector3 forseActingOnFirst;
        }
    }
}
