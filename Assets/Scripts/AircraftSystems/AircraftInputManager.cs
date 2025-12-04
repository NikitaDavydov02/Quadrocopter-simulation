using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PlaneController planeController;
    [SerializeField]
    private UIManager UIManager;
    [SerializeField]
    private GearManager gearManager;
    //public int FlapsSetPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.PageUp))
            planeController.Trim(1.0f);
        if (Input.GetKey(KeyCode.PageDown))
            planeController.Trim(-1.0f);
        if (Input.GetKeyDown(KeyCode.Alpha0))
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
            Flaps(8);

        if (Input.GetKeyDown(KeyCode.G))
            GearInput();

        if (Input.GetKeyDown(KeyCode.I))
            UIManager.ToggleUIPanel();

    }
    public void Flaps(int pos)
    {
        planeController.Flaps(pos);
        UIManager.Flaps(pos);
    }
    public void GearInput()
    {
        if (gearManager.inProgress)
            return;
        planeController.ToggleGear();
        UIManager.ToggleGear(gearManager.extended);
    }
}
