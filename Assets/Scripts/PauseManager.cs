using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public AudioMixerSnapshot normal;
    [SerializeField]
    public AudioMixerSnapshot paused;
    bool normalB = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Ois pressed");
            if (normalB)
                paused.TransitionTo(0.01f);
            else
                normal.TransitionTo(0.01f);
            normalB = !normalB;
        }
    }
}
