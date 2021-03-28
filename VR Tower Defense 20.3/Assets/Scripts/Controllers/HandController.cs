using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        // InputDeviceCharacteristics rchara = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        // InputDevices.GetDevicesWithCharacteristics(rchara, devices);

        // print("Number of devices: " + devices.Count);
        foreach (var device in devices)
        {
            print(device.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelectEntered()
    {
        // print("selected");
    }
    
    public void OnSelectExited()
    {
        // print("exited");
    }
}
