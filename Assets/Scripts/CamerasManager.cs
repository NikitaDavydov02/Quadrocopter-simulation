using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class CamerasManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private List<GameObject> cinemachineCameras;
    private int currentCamera = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnOffFreeLookCamera()
    {
        //freeLookCamera.m_XAxis.Value = 0f;
        //freeLookCamera.m_YAxis.Value = 0.5f;
        freeLookCamera.m_RecenterToTargetHeading.m_enabled = true;
        freeLookCamera.m_YAxisRecentering.m_enabled = true;
    }
    public void TurnOnFreeLookCamera()
    {
        freeLookCamera.m_RecenterToTargetHeading.m_enabled = false;
        freeLookCamera.m_YAxisRecentering.m_enabled = false;
    }
    public void SetFreeLookCameraInputActionreference(InputActionReference reference)
    {
        CinemachineInputProvider cinemachineInputProvider = freeLookCamera.GetComponent<CinemachineInputProvider>();
        cinemachineInputProvider.XYAxis = reference;
    }
    public void SwitchCamera()
    {
        cinemachineCameras[currentCamera].SetActive(false);
        currentCamera++;
        if(currentCamera == cinemachineCameras.Count)
            currentCamera= 0;
        cinemachineCameras[currentCamera].SetActive(true);

    }
}
