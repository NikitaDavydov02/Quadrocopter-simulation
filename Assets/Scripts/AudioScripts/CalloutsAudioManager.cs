using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalloutsAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float lastAltitude;
    public float currentAltitude;
    [SerializeField]
    private List<float> limits;
    [SerializeField]
    private List<AudioClip> limitClips;
    [SerializeField]
    private AudioSource calloutsAudioSource;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(new Ray(transform.position,Vector3.down),out hit))
        {
            currentAltitude = hit.distance-1.18f;
        }
        for(int i=0;i<limits.Count;i++)
        {
            if(currentAltitude<=limits[i]&&lastAltitude>limits[i])
            {
                if(!calloutsAudioSource.isPlaying)
                    calloutsAudioSource.PlayOneShot(limitClips[i]);
            }
        }

        lastAltitude = currentAltitude;
    }
}
