using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(menuName = "ScriptableObjects/Devices")]
public class Devices : ScriptableObject
{
    public bool isReady = false;
    public InputDevice rightHand;
    public InputDevice leftHand;

    [SerializeField] private DevicesReadyEvent _devicesReadyEvent;
    
    public void InitDevices(InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        var baseChara = InputDeviceCharacteristics.Controller | 
                        InputDeviceCharacteristics.TrackedDevice |
                        InputDeviceCharacteristics.HeldInHand;
        var rightCtrlChara = (baseChara | InputDeviceCharacteristics.Right);
        var leftCtrlChara  = (baseChara | InputDeviceCharacteristics.Left);
        
        InputDevices.GetDevicesWithCharacteristics(rightCtrlChara, devices);
        if (devices.Count > 0)
        {
            rightHand = devices[0];
        }
        
        InputDevices.GetDevicesWithCharacteristics(leftCtrlChara, devices);
        if (devices.Count > 0)
        {
            leftHand = devices[0];
        }

        isReady = true;
        _devicesReadyEvent.Raise();
    }
}
