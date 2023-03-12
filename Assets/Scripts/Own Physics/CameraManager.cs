using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Transform> targets;
    public List<Vector3> offcets;
    private int currentTarget = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (targets != null)
        {
            if (targets.Count != 0)
                currentTarget = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentTarget != -1)
            {
                if (currentTarget < targets.Count - 1)
                    currentTarget++;
                else
                    currentTarget = 0;
            }
        }
        if (currentTarget != -1)
        {
            Vector3 cameraPos = targets[currentTarget].transform.position + targets[currentTarget].transform.TransformDirection(offcets[currentTarget]);
            this.transform.position = cameraPos;
            transform.LookAt(targets[currentTarget].transform);
        }
    }
}
