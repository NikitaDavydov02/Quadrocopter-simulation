using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearsAudioManager : MonoBehaviour
{
    Collider collider;
    [SerializeField]
    AudioSource gearsAudioSource;
    [SerializeField]
    AudioClip gearsTouch;
    [SerializeField]
    AudioClip gearsRolling;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        gearsAudioSource.PlayOneShot(gearsTouch);
        Debug.Log("Collide");
    }
    private void OnCollisionStay(Collision collision)
    {
        if (gearsAudioSource.clip != gearsRolling && rb.velocity.magnitude>0.1f)
        {
            gearsAudioSource.clip = gearsRolling;
            gearsAudioSource.loop = true;
            gearsAudioSource.Play();
        }
        Debug.Log("Roll");
    }
}
