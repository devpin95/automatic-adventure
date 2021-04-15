using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Devices _devices;
    [SerializeField] private DevicesReadyEvent _devicesReadyEvent;
    // Start is called before the first frame update
    void Start()
    {
        InputDevices.deviceConnected += InitDevices;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_devices.IsReady)
        {
            Debug.Log("Device Not Ready");
        }
    }

    private void InitDevices(InputDevice inputDevice)
    {
        // _devicesReadyEvent.Raise();
        _devices.InitDevices(inputDevice);
        Debug.Log("GAME MANAGER: Init Devices");
    }
}
