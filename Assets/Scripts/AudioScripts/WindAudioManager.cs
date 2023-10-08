using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private AudioClip windAudioClip;
    [SerializeField]
    private AudioSource windAudioSource;
    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {
        windAudioSource.pitch = rb.velocity.magnitude / 100;
    }
}
