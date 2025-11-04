using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip engineAudioClip;
    [SerializeField]
    private AudioClip reverseAudioClip;
    [SerializeField]
    private AudioSource engineAudioSource;
    [SerializeField]
    private AudioSource reverseAudioSource;
    public float rotationlevel = 0f;
    public float reverseDegree = 0f;
    public float minPitch = 1f;
    public float maxPitch = 2f;
    public float minVolume = 0f;
    public float maxVolume = 2f;
    public float maxReverseVolme = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        engineAudioSource.clip = engineAudioClip;
        engineAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (engineAudioSource.clip != engineAudioClip)
        {
            engineAudioSource.clip = engineAudioClip;
            engineAudioSource.Play();
        }
        engineAudioSource.pitch = minPitch + (maxPitch - minPitch) * rotationlevel;
        engineAudioSource.volume = (maxVolume - minVolume) * rotationlevel;

        if (reverseDegree > 0)
        {
            if (reverseAudioSource.clip != reverseAudioClip)
            {
                reverseAudioSource.clip = reverseAudioClip;

                //engineAudioSource.volume = 0.25f;
                reverseAudioSource.Play();
            }
            reverseAudioSource.pitch = 1;
            reverseAudioSource.volume = maxReverseVolme * (reverseDegree*rotationlevel);
        }
        else
            reverseAudioSource.volume = 0;
    }
}
