using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip rollClip;
    [SerializeField]
    private AudioSource source;
    public bool roll = false;

    public float ReferenceSpeed;
    public float CurrentSpeed;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = rollClip;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        source.enabled = roll;
        source.pitch = CurrentSpeed / ReferenceSpeed;
    }
    
}
