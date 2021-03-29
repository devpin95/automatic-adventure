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

    [Header("Scriptable Objects")] 
    [SerializeField] private Devices _devices;

    private int shotCount = 0;
    private Vector3 startingPosition;
    private Vector3 centerPosition;
    private bool selected = false;
    private bool firing = false;
    private float timeSinceLastShot = 0.0f;
    private XRBaseInteractor _currentInteractor;

    // private InputDevice _rightHand;
    // private InputDevice _leftHand;
    private bool _rightHandSupportsHaptics = false;
    private bool _leftHandSupportsHaptics = false;

    private AimController _aimController;
    private TowerRotateController _towerRotateController;


    // Start is called before the first frame update
    void Start()
    {
        tempHand.SetActive(false);
        startingPosition = pivotPoint.transform.position;
        _aimController = GetComponent<AimController>();
        _towerRotateController = GetComponent<TowerRotateController>();
    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = transform.position;
        if (selected && _devices.isReady)
        {
            _aimController.AimWeapon(_currentInteractor, centerPosition);

            if (firing)
            {
                timeSinceLastShot += Time.deltaTime;

                if (timeSinceLastShot > shotDelay)
                {
                    ShootMachinegun();

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
            
            _towerRotateController.RotateTower();
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

    public void OnDeviceConnectedResponse()
    {
        Debug.Log("Machine gun ready for device!");
    }
    
    private void ShootMachinegun()
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
    }
    
}
