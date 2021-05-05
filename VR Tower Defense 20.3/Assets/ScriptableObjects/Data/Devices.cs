using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

static class Constants
{
    public const string RIGHT_HAND_TAGNAME = "Right Hand Interactor";
    public const string LEFT_HAND_TAGNAME = "Left Hand Interactor";
}

[CreateAssetMenu(menuName = "ScriptableObjects/Devices")]
public class Devices : ScriptableObject
{
    private bool isReady = false;

    public bool IsReady
    {
        get => isReady;
        private set => isReady = value;
    }
    
    private InputDevice rightHand;
    public InputDevice RightHand
    {
        get => rightHand;
        private set => rightHand = value;
    }
    
    private InputDevice leftHand;
    public InputDevice LeftHand
    {
        get => leftHand;
        private set => leftHand = value;
    }

    private bool leftHandActive = false;
    public bool LeftHandActive
    {
        get => leftHandActive;
        set => leftHandActive = value;
    }

    private bool rightHandActive = false;

    public bool RightHandActive
    {
        get => rightHandActive;
        set => rightHandActive = value;
    }

    private bool rightHandSupportsHaptics;
    public bool RightHandSupportsHaptics
    {
        get => rightHandSupportsHaptics;
        private set => rightHandSupportsHaptics = value;
    }
    
    private bool leftHandSupportsHaptics;
    public bool LeftHandSupportsHaptics
    {
        get => leftHandSupportsHaptics;
        private set => leftHandSupportsHaptics = value;
    }

    [SerializeField] private CEvent _devicesReadyEvent;
    
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
            RightHand = devices[0];
        }
        
        InputDevices.GetDevicesWithCharacteristics(leftCtrlChara, devices);
        if (devices.Count > 0)
        {
            LeftHand = devices[0];
        }

        HapticCapabilities haptic_capable;
        RightHandSupportsHaptics = rightHand.TryGetHapticCapabilities(out haptic_capable) && haptic_capable.supportsImpulse;
        LeftHandSupportsHaptics =
            LeftHand.TryGetHapticCapabilities(out haptic_capable) && haptic_capable.supportsImpulse;

        IsReady = true;
        _devicesReadyEvent.Raise();
    }

    public void SetSelectStateByTagname(string tag, bool state)
    {
        if (tag == Constants.RIGHT_HAND_TAGNAME)
        {
            RightHandActive = state;
        } 
        else if (tag == Constants.LEFT_HAND_TAGNAME)
        {
            LeftHandActive = state;
        }
    }

    public void ResetObject()
    {
        IsReady = false;
        RightHandActive = false;
        LeftHandActive = false;
    }
}
