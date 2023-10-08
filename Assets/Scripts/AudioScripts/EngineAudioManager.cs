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
    public float level = 0f;
    public float minPitch = 1f;
    public float maxPitch = 2f;
    public float minVolume = 0f;
    public float maxVolume = 2f;

    // Start is called before the first frame update
    void Start()
    {
        engineAudioSource.clip = engineAudioClip;
        engineAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (level >= 0)
        {
            if (engineAudioSource.clip != engineAudioClip)
            {
                engineAudioSource.clip = engineAudioClip;
                engineAudioSource.Play();
            }
            engineAudioSource.pitch = minPitch + (maxPitch - minPitch) * level;
            engineAudioSource.volume = (maxVolume - minVolume) * level;

        }
        else 
        {
            if(engineAudioSource.clip!=reverseAudioClip)
            {
                engineAudioSource.clip = reverseAudioClip;
                engineAudioSource.pitch = 1;
                engineAudioSource.volume = 1;
                engineAudioSource.Play();
            }
            
        }
    }
}
