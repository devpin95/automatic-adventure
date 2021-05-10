using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticsManager : MonoBehaviour
{
    public Devices _devices;

    // private bool _waitingOnOneShot = false;

    private float _indefiniteRumbleTimeout = 0.1f;

    private bool _rumbleRight = false;
    private bool _rumbleLeft = false;
    private bool _indefiniteRumbleRight = false;
    private bool _indefiniteRumbleLeft = false;

    private float _currentLeftAmplitude;
    private float _currentRightAmplitude;

    private float _currentLeftDuration;
    private float _currentRightDuration;

    private bool _deviceReadyToRead = false;
    private List<UnityEngine.XR.InputDevice> supportedDevices = new List<UnityEngine.XR.InputDevice>();
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!_deviceReadyToRead) return;

        if (!_rumbleRight && _indefiniteRumbleRight)
        {
            _currentRightDuration = _indefiniteRumbleTimeout;
            StartCoroutine(StartRightRumble());
        }

        if (!_rumbleLeft && _indefiniteRumbleLeft)
        {
            _currentLeftDuration = _indefiniteRumbleTimeout;
            StartCoroutine(StartLeftRumble());
        }
    }
    
    public bool RequestIndefiniteRumble(UnityEngine.XR.InputDevice device, float amplitude)
    {
        if (!supportedDevices.Contains(device)) return false;

        if (device.Equals(_devices.RightHand) && !_indefiniteRumbleRight)
        {
            _indefiniteRumbleRight = true;
            _currentRightAmplitude = amplitude;
            return true;
        }
        else if (device.Equals(_devices.LeftHand) && !_indefiniteRumbleLeft)
        {
            _indefiniteRumbleLeft = true;
            _currentLeftAmplitude = amplitude;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RequestOneOffRumble(string deviceTag, float amplitude, float duration)
    {
        if (deviceTag == Constants.RIGHT_HAND_TAGNAME)
            return RequestOneOffRumble(_devices.RightHand, amplitude, duration);
        else if (deviceTag == Constants.LEFT_HAND_TAGNAME) 
            return RequestOneOffRumble(_devices.LeftHand, amplitude, duration);

        return false;
    }
    
    public bool RequestOneOffRumble(UnityEngine.XR.InputDevice device, float amplitude, float duration)
    {
        if (!supportedDevices.Contains(device)) return false;
        
        if (device.Equals(_devices.RightHand) && !_indefiniteRumbleRight)
        {
            if (_rumbleRight) return false;
            
            _currentRightDuration = duration;
            _currentRightAmplitude = amplitude;
            StartCoroutine(StartRightRumble());
            return true;
        }
        else if (device.Equals(_devices.LeftHand) && !_indefiniteRumbleLeft)
        {
            if (_rumbleLeft) return false;
            
            _currentLeftDuration = duration;
            _currentLeftAmplitude = amplitude;
            StartCoroutine(StartLeftRumble());
            return true;
        }
        
        return false;
    }

    public void StopIndefiniteRumble(UnityEngine.XR.InputDevice device)
    {
        if (device.Equals(_devices.RightHand)) _indefiniteRumbleRight = false;
        else if (device.Equals(_devices.LeftHand)) _indefiniteRumbleLeft = false;
    }

    public void GetDeviceData()
    {
        _deviceReadyToRead = true;
        supportedDevices.Add(_devices.LeftHand);
        supportedDevices.Add(_devices.RightHand);
    }

    IEnumerator StartRightRumble()
    {
        _devices.RightHand.SendHapticImpulse(1, _currentRightAmplitude, _currentRightDuration);
        _rumbleRight = true;
        yield return new WaitForSeconds(_currentRightDuration);
        _rumbleRight = false;
    }
    
    IEnumerator StartLeftRumble()
    {
        _devices.RightHand.SendHapticImpulse(1, _currentLeftAmplitude, _currentLeftDuration);
        _rumbleLeft = true;
        yield return new WaitForSeconds(_currentLeftDuration);
        _rumbleLeft = false;
    }
}
