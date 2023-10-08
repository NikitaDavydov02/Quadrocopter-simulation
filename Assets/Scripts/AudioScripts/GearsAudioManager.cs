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
    [SerializeField]
    AudioClip gearsUp;
    [SerializeField]
    AudioClip gearsDown;
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
        if ((gearsAudioSource.clip != gearsRolling|| !gearsAudioSource.isPlaying )&& rb.velocity.magnitude>0.1f)
        {
            gearsAudioSource.clip = gearsRolling;
            gearsAudioSource.loop = true;
            gearsAudioSource.Play();
        }
        gearsAudioSource.pitch = rb.velocity.magnitude / 70;
        Debug.Log("Roll");
    }
    private void OnCollisionExit(Collision collision)
    {
        gearsAudioSource.Stop();
    }
    public void GearsUp()
    {
        gearsAudioSource.PlayOneShot(gearsUp);
    }
    public void GearsDown()
    {
        gearsAudioSource.PlayOneShot(gearsDown);
    }
}
