using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MachineGunController : MonoBehaviour
{
    [Header("Meta Objects")]
    public Transform fireLocation;
    public Transform pivotPoint;
    public GameObject tempHand;
    
    [Header("Shooting")]
    public GameObject tracerPrefab;
    public GameObject bulletPrefab;
    public float shotDelay = 0.05f;
    public int tracerSpacing;
    public float bulletSpeedModifier = 400f;

    [Header("Rotation")]
    public GameObject playerRig;
    public GameObject towerTrans;

    public float rotateSpeed;
    
    private int shotCount = 0;
    private Vector3 startingPosition;
    private Vector3 centerPosition;
    private bool selected = false;
    private bool firing = false;
    private float timeSinceLastShot = 0.0f;
    private XRBaseInteractor _currentInteractor;

    private InputDevice _rightHand;
    private InputDevice _leftHand;
    private bool _rightHandSupportsHaptics = false;
    private bool _leftHandSupportsHaptics = false;


    // Start is called before the first frame update
    void Start()
    {
        tempHand.SetActive(false);
        startingPosition = pivotPoint.transform.position;
        InputDevices.deviceConnected += OnDeviceConnected;
    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = transform.position;
        if (selected)
        {
            Vector3 handPos = _currentInteractor.transform.position;
            Vector3 centerToHand = handPos - centerPosition;
            transform.rotation = Quaternion.LookRotation(-centerToHand);
            Vector3 angles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(angles.x + 90, angles.y, angles.z);

            if (firing)
            {
                timeSinceLastShot += Time.deltaTime;

                if (timeSinceLastShot > shotDelay)
                {
                    ++shotCount;
                    timeSinceLastShot = 0;
                    float ranXrot = UnityEngine.Random.Range(-1.0f, 1.0f);
                    float ranYrot = UnityEngine.Random.Range(-1.0f, 1.0f);
                    Vector3 randDir = Quaternion.Euler(ranXrot, ranYrot,  0) * fireLocation.forward;

                    GameObject bullet;
                    if (shotCount % tracerSpacing == 0)
                    {
                        // shoot a tracer
                        bullet = Instantiate(tracerPrefab, fireLocation.position, transform.rotation);
                    }
                    else
                    {
                        // shoot a regular bullet
                        bullet = Instantiate(bulletPrefab, fireLocation.position, transform.rotation);
                    }
                    
                    bullet.GetComponent<Rigidbody>().AddForce(randDir * bulletSpeedModifier, ForceMode.Impulse);

                    // if (_rightHandSupportsHaptics || _leftHandSupportsHaptics)
                    // {
                    //     if (_currentInteractor.CompareTag("Right Hand Interactor"))
                    //     {
                    //         _rightHand.SendHapticImpulse(1, 0.5f, 0.1f);
                    //     } 
                    //     else if (_currentInteractor.CompareTag("Left Hand Interactor"))
                    //     {
                    //         _leftHand.SendHapticImpulse(1, 0.5f, 0.1f);
                    //     }
                    // }
                }
            }

            Vector2 joystickVal;
            if (_rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickVal))
            {
                // print(joystickVal);
                // playerRig.transform.Rotate(0, joystickVal.x * rotateSpeed * Time.deltaTime, 0);
                towerTrans.transform.RotateAround(playerRig.transform.position, playerRig.transform.up, joystickVal.x * rotateSpeed * Time.deltaTime);
                transform.RotateAround(playerRig.transform.position, playerRig.transform.up, joystickVal.x * rotateSpeed * Time.deltaTime);
            }
        }
    }

    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        selected = true;
        tempHand.SetActive(true);
        _currentInteractor = interactor;
        print(_currentInteractor.tag);
    }

    public void OnSelectExit(XRBaseInteractor interactor)
    {
        selected = false;
        tempHand.SetActive(false);
        _currentInteractor = null;
    }

    public void OnActivate(XRBaseInteractor interactor)
    {
        firing = true;
    }
    
    public void OnDeactivate(XRBaseInteractor interactor)
    {
        firing = false;
    }

    public void OnDeviceConnected(InputDevice device)
    {
        InitDevice();
    }
    
    private void InitDevice()
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
            _rightHand = devices[0];
            HapticCapabilities hapcap = new HapticCapabilities();
            _rightHand.TryGetHapticCapabilities(out hapcap);

            if (hapcap.supportsImpulse)
            {
                print("Right hand can impulse");
                _rightHandSupportsHaptics = true;
            }
        }
        
        InputDevices.GetDevicesWithCharacteristics(leftCtrlChara, devices);
        if (devices.Count > 0)
        {
            _leftHand = devices[0];
            HapticCapabilities hapcap = new HapticCapabilities();
            _leftHand.TryGetHapticCapabilities(out hapcap);

            if (hapcap.supportsImpulse)
            {
                print("Left hand can impulse");
                _leftHandSupportsHaptics = true;
            }
        }
    }
}
