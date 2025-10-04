using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirfieldCamera : MonoBehaviour
{
    [SerializeField]
    Transform target;
    float size = 40f;
    [SerializeField]
    float frameFraction = 0.5f;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            float distance = (target.transform.position - transform.position).magnitude;
            this.gameObject.transform.LookAt(target);
            float angle = Mathf.Rad2Deg * Mathf.Atan(size / (frameFraction * distance));


            camera.fieldOfView = angle;
        }
    }
}
