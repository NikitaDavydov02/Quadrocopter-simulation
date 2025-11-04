using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapsAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;
    private float inductionRate = 2f;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        //audioSource.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
            audioSource.pitch += inductionRate * Time.deltaTime;
        else
            audioSource.pitch -= inductionRate * Time.deltaTime;
        if (audioSource.pitch > 1)
            audioSource.pitch = 1;
        if (audioSource.pitch < 0)
            audioSource.pitch = 0;
    }
    public void StartFlaps()
    {
        Debug.Log("Flaps on");
        active = true;
    }
    public void StopFlaps()
    {
        Debug.Log("Flaps off");
        active = false;
    }
}
