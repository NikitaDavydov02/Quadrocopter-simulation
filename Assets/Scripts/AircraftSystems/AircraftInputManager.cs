using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AircraftInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PlaneController planeController;
    [SerializeField]
    private UIManager UIManager;
    [SerializeField]
    private GearManager gearManager;
    //[SerializeField]
    //private CinemachineInputProvider cinemachineInputProvider;
    [SerializeField]
    private CamerasManager camerasManager;  

    private PlaneInputAction planeInput;
    void Start()
    {
        planeInput = new PlaneInputAction();
        //planeInput = cinemachineInputProvider.XYAxis.action.actionMap.asset;

        planeInput.Menu.Disable();
        planeInput.FreeLookCamera.Disable();
        planeInput.PlaneFlight.Enable();

        //InputActionReference actionRef = new InputActionReference();
        //actionRef = planeInput.FreeLookCamera.CustomLook.
        //actionRef.action = planeInput.FreeLookCamera.CustomLook;

        InputActionReference reference = InputActionReference.Create(planeInput.FreeLookCamera.CustomLook);
        //cinemachineInputProvider.XYAxis = reference;
        camerasManager.SetFreeLookCameraInputActionreference(reference);

        planeInput.PlaneFlight.Brake.started += Brake_started;
        planeInput.PlaneFlight.Brake.canceled += Brake_canceled;

        planeInput.PlaneFlight.Reverses.performed += Reverses_performed;

        planeInput.PlaneFlight.Lights.performed += Lights_performed;

        planeInput.PlaneFlight.Gears.performed += Gears_performed;

        planeInput.PlaneFlight.FlightInstruments.performed += FlightInstruments_performed;

        planeInput.PlaneFlight.Flaps.performed += Flaps_performed;
        //planeInput.PlaneFlight.Thrust.performed += Thrust;        
        planeInput.PlaneFlight.OpenMenu.performed += OpenMenu_performed;

        planeInput.PlaneFlight.FreelLookCamera.performed += FreelLookCamera_performed;

        planeInput.PlaneFlight.SwitchCamera.performed += SwitchCamera_performed;

        planeInput.Menu.CloseMenu.performed += CloseMenu_performed;

        planeInput.FreeLookCamera.TurnOffFreeLookCamera.performed += TurnOffFreeLookCamera_performed;

    }

    private void SwitchCamera_performed(InputAction.CallbackContext obj)
    {
        camerasManager.SwitchCamera();
    }

    private void TurnOffFreeLookCamera_performed(InputAction.CallbackContext obj)
    {
        planeInput.PlaneFlight.Enable();
        planeInput.FreeLookCamera.Disable();

        camerasManager.TurnOffFreeLookCamera();
    }

    private void FreelLookCamera_performed(InputAction.CallbackContext obj)
    {
        planeInput.PlaneFlight.Disable();
        planeInput.FreeLookCamera.Enable();
        
        camerasManager.TurnOnFreeLookCamera();
    }

    private void CloseMenu_performed(InputAction.CallbackContext obj)
    {
        UIManager.OpenCloseMenu();
        planeInput.PlaneFlight.Enable();
        planeInput.Menu.Disable();
    }

    private void OpenMenu_performed(InputAction.CallbackContext obj)
    {
        UIManager.OpenCloseMenu();
        planeInput.PlaneFlight.Disable();
        planeInput.Menu.Enable();
    }

    private void Flaps_performed(InputAction.CallbackContext obj)
    {
        int value = (int)obj.ReadValue<float>();
        Flaps(value);
    }

    private void FlightInstruments_performed(InputAction.CallbackContext obj)
    {
        UIManager.ToggleUIPanel();
    }

    private void Gears_performed(InputAction.CallbackContext obj)
    {
        if (gearManager.inProgress)
            return;
        planeController.ToggleGear();
        UIManager.ToggleGear(gearManager.extended);
    }

    private void Lights_performed(InputAction.CallbackContext obj)
    {
        planeController.Lights();
    }

    private void Reverses_performed(InputAction.CallbackContext obj)
    {
        planeController.Reverses();
    }

    private void Brake_canceled(InputAction.CallbackContext obj)
    {
        planeController.BrakeOut();
    }

    private void Brake_started(InputAction.CallbackContext obj)
    {
        planeController.BrakeIn();
    }

    // Update is called once per frame
    void Update()
    {
        planeController.Thrust(planeInput.PlaneFlight.Thrust.ReadValue<float>());
        planeController.Ailerons(planeInput.PlaneFlight.Ailerons.ReadValue<float>());
        float y_valye = planeInput.PlaneFlight.Elevator.ReadValue<float>(); 
        planeController.Elevator(y_valye);
        float x_valye = planeInput.PlaneFlight.Rudder.ReadValue<float>();
        planeController.Rudder(x_valye);
        planeController.SteeringWheel(x_valye);

        planeController.Trim(planeInput.PlaneFlight.Trim.ReadValue<float>());

        /*if (Input.GetKeyDown(KeyCode.Alpha0))
            Flaps(0);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Flaps(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Flaps(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Flaps(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Flaps(4);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            Flaps(5);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            Flaps(6);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            Flaps(7);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            Flaps(8);*/


    }
    public void Flaps(int pos)
    {
        planeController.Flaps(pos);
        UIManager.Flaps(pos);
    }
}
