using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using InputDevice = UnityEngine.XR.InputDevice;

public class MachineGunController : MonoBehaviour
{
    public MachineGunUpgrades machineGunUpgrades;
    [Header("Meta Objects")]
    public Transform fireLocation;
    public GameObject tempHand;
    public Transform handleAnchor;
    public GameObject handleInteractor;
    public Transform gunCenter;

    public CEvent_BountyTrackedData bountyNotification;
    public BountyTrackedData bountyTrackedData = new BountyTrackedData();
    private string _fireRoundBountyID = "Machinegun Fire";
    private string _reloadBountyID = "Machinegun Reload";
    
    [Header("Shooting")]
    public float shotDelay = 0.05f;
    public int tracerSpacing;
    [Tooltip("Overwritten by MachineGunUpdates")]

    private HapticsManager _haptics;

    [Header("Scriptable Objects")] 
    [SerializeField] private Devices _devices;

    [Header("Input Actions")] 
    public CEvent_Int switchMapEvent;
    public InputActionReference fire;
    // fire.action.performed += IAFire;

    private int _shotCount = 0; // keeps track of when to shoot a tracer
    private bool initialLoad = true;
    private bool _ammoBoxInstalled = true;
    // private MachineGunAmmoBox _activeAmmoBox; // the current ammo box to take ammo from
    private AmmoBoxController _activeAmmoBox;
    private Vector3 _restingPosition;
    private Quaternion _restingRotation; // the rotation to keep when we let go of the gun
    private Vector3 _centerPosition; // the center of the gun to rotate around when aiming
    private bool _selected = false; // if the gun grip is being held or not
    private bool _firing = false; // if the gun grip is being held and the trigger is being pulled
    private float _timeSinceLastShot = 0.0f; // the elapsed time since the last shot was fired
    private XRBaseInteractor _currentInteractor; // the interactor that is currently holding the gun grip
    private InputDevice _currentDevice; // the device (controller) that is currently interacting with the gun

    private AimController _aimController; // script to aim the gun
    private TowerRotateController _towerRotateController; // script to rotate the tower
    private AudioSource _shootingSound; // sound to play when the gun is firing
    
    
    void Start()
    {
        tempHand.SetActive(false);
        _restingPosition = transform.position;
        _restingRotation = transform.rotation;
        _aimController = GetComponent<AimController>();
        _towerRotateController = GetComponent<TowerRotateController>();
        _shootingSound = GetComponent<AudioSource>();
        _haptics = GameObject.Find("HapticsManager").GetComponent<HapticsManager>();
        // _bulletPool = GetComponent<ObjectPool>();

        
        fire.action.performed += IAFire;

        _centerPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // centerPosition = transform.position;
        
        handleInteractor.transform.position = handleAnchor.position;
        handleInteractor.transform.rotation = handleAnchor.rotation;

        transform.localPosition = _centerPosition;
        if (_selected && _devices.IsReady && machineGunUpgrades.initialized)
        {
            _aimController.AimWeapon(_currentInteractor, gunCenter.position);
            _restingRotation = transform.rotation;
            
            if (_firing && _ammoBoxInstalled)
            {
                _timeSinceLastShot += Time.deltaTime;

                if (_timeSinceLastShot > shotDelay)
                {
                    // Debug.Log("Shooting");
                    ShootMachinegun();
                }
            }
            
            _towerRotateController.RotateTower(machineGunUpgrades.TowerRotationSpeed);
            _restingPosition = transform.position;
        }
        else
        {
            transform.rotation = _restingRotation;
            // transform.position = restingPosition;
        }
    }

    public void OnGripSelectEnter(XRBaseInteractor interactor)
    {
        _selected = true;
        tempHand.SetActive(true);
        _currentInteractor = interactor;

        if (_currentInteractor.CompareTag("Left Hand Interactor")) _currentDevice = _devices.LeftHand;
        else _currentDevice = _devices.RightHand;
        
        switchMapEvent.Raise((int)PlayerActionStateManager.ActionMap.Machinegun);
    }

    public void OnGripSelectExit(XRBaseInteractor interactor)
    {
        _selected = false;
        _firing = false;
        _haptics.StopIndefiniteRumble(_currentDevice);
        tempHand.SetActive(false);
        _currentInteractor = null;
        _shootingSound.Stop();
        switchMapEvent.Raise((int)PlayerActionStateManager.ActionMap.Freemovement);
    }

    public void OnGripActivate(XRBaseInteractor interactor)
    {
        if (_ammoBoxInstalled)
        {
            _firing = true;
            _shootingSound.Play();
        }
    }
    
    public void OnGripDeactivate(XRBaseInteractor interactor)
    {
        _firing = false;
        // _haptics.StopIndefiniteRumble(_currentDevice);
        _shootingSound.Stop();
    }

    public void OnAmmoLoaded(XRBaseInteractable interactable)
    {
        // Debug.Log("Ammo Loaded");
        AmmoBoxController ammoBoxController = interactable.GetComponent<AmmoBoxController>();
        if (ammoBoxController)
        {
            if (ammoBoxController != _activeAmmoBox)
            {
                bountyTrackedData.trackedDataId = _reloadBountyID;
                bountyTrackedData.bountyType = Bounty.BountyTypes.Action;
                bountyNotification.Raise(bountyTrackedData);
            }
            _activeAmmoBox = ammoBoxController;
            // Debug.Log("AMMO: " + _activeAmmoBox.Count + "/" + _activeAmmoBox.Capacity);
            _ammoBoxInstalled = true;
        }
    }

    public void OnAmmoUnloaded(XRBaseInteractable interactable)
    {
        _activeAmmoBox = null;
        _ammoBoxInstalled = false;
        _firing = false;
        _shootingSound.Stop();
    }

    public void OnDeviceConnectedResponse()
    {
        Debug.Log("Machine gun ready for device!");
    }
    
    private void ShootMachinegun()
    {
        if (_activeAmmoBox.Count <= 0)
        {
            // Debug.Log("The ammo box is empty");
            _shootingSound.Stop();
            return;
        }
        
        bountyTrackedData.trackedDataId = _fireRoundBountyID;
        bountyTrackedData.bountyType = Bounty.BountyTypes.Action;
        bountyNotification.Raise(bountyTrackedData);
        
        --_activeAmmoBox.Count;
        
        GameObject bullet = MachineGunBulletPool.SharedInstance.GetPooledObject();

        if (bullet == null) return;
        
        var trail = bullet.transform.Find("Trail").GetComponent<TrailRenderer>();
        trail.enabled = false;
        
        bullet.transform.position = fireLocation.position;
        bullet.transform.rotation = transform.rotation;
        // bullet.transform.rotation = Quaternion.AngleAxis(90, bullet.transform.right);

        ++_shotCount;
        _timeSinceLastShot = 0;
        float ranXrot = UnityEngine.Random.Range(-1.0f, 1.0f);
        float ranYrot = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 randDir = Quaternion.Euler(ranXrot, ranYrot,  0) * fireLocation.forward;
        
        BulletController bulletController = bullet.GetComponent<BulletController>();

        //GameObject bullet;
        if (_shotCount % tracerSpacing == 0)
        {
            bulletController.MakeTracerRound();
            bulletController.TracerColor(bulletController.defaultTracerColor);
        }
        else bulletController.MakeDefaultRound();
        
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddForce(randDir.normalized * machineGunUpgrades.BulletVelocityModifier, ForceMode.Impulse);
    }

    public void IAFire(InputAction.CallbackContext context)
    {
        if (!_selected) return;
        
        bool buttonPressed = context.ReadValueAsButton();
        if (buttonPressed)
        {
            if (_ammoBoxInstalled)
            {
                _firing = true;
                _shootingSound.Play();
            }
        }
        else
        {
            _firing = false;
            // _haptics.StopIndefiniteRumble(_currentDevice);
            _shootingSound.Stop();
        }
    }

    private void EnableInputAction()
    {
        fire.action.Enable();
    }

    private void DisableInputActions()
    {
        fire.action.Disable();
    }
}
