using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MachineGunController : MonoBehaviour
{
    public MachineGunUpgrades machineGunUpgrades;
    [Header("Meta Objects")]
    public Transform fireLocation;
    public Transform pivotPoint;
    public GameObject tempHand;
    
    [Header("Shooting")]
    public GameObject tracerPrefab;
    public GameObject bulletPrefab;
    public float shotDelay = 0.05f;
    public int tracerSpacing;
    [Tooltip("Overwritten by MachineGunUpdates")]

    private HapticsManager _haptics;

    [Header("Scriptable Objects")] 
    [SerializeField] private Devices _devices;

    private int shotCount = 0;
    private Vector3 restingPosition;
    private Quaternion restingRotation;
    private Vector3 centerPosition;
    private bool selected = false;
    private bool firing = false;
    private float timeSinceLastShot = 0.0f;
    private XRBaseInteractor _currentInteractor;
    private InputDevice _currentDevice;

    private AimController _aimController;
    private TowerRotateController _towerRotateController;
    private AudioSource _shootingSound;

    // private ObjectPool _bulletPool;

    // private Vector3 fireDebug;


    // Start is called before the first frame update
    void Start()
    {
        tempHand.SetActive(false);
        restingPosition = transform.position;
        restingRotation = transform.rotation;
        _aimController = GetComponent<AimController>();
        _towerRotateController = GetComponent<TowerRotateController>();
        _shootingSound = GetComponent<AudioSource>();
        _haptics = GameObject.Find("HapticsManager").GetComponent<HapticsManager>();
        // _bulletPool = GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        centerPosition = transform.position;
        if (selected && _devices.IsReady && machineGunUpgrades.initialized)
        {
            _aimController.AimWeapon(_currentInteractor, centerPosition);
            restingRotation = transform.rotation;

            if (firing)
            {
                timeSinceLastShot += Time.deltaTime;

                if (timeSinceLastShot > shotDelay)
                {
                    ShootMachinegun();
                }
            }
            
            _towerRotateController.RotateTower(machineGunUpgrades.TowerRotationSpeed);
            restingPosition = transform.position;
        }
        else
        {
            transform.rotation = restingRotation;
            transform.position = restingPosition;

        }
    }

    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        selected = true;
        tempHand.SetActive(true);
        _currentInteractor = interactor;

        if (_currentInteractor.CompareTag("Left Hand Interactor")) _currentDevice = _devices.LeftHand;
        else _currentDevice = _devices.RightHand;
        
    }

    public void OnSelectExit(XRBaseInteractor interactor)
    {
        selected = false;
        firing = false;
        _haptics.StopIndefiniteRumble(_currentDevice);
        tempHand.SetActive(false);
        _currentInteractor = null;
        _shootingSound.Stop();
    }

    public void OnActivate(XRBaseInteractor interactor)
    {
        firing = true;

        // _haptics.RequestIndefiniteRumble(_currentDevice, 0.5f);
        _shootingSound.Play();
    }
    
    public void OnDeactivate(XRBaseInteractor interactor)
    {
        firing = false;
        // _haptics.StopIndefiniteRumble(_currentDevice);
        _shootingSound.Stop();
    }

    public void OnDeviceConnectedResponse()
    {
        Debug.Log("Machine gun ready for device!");
    }
    
    private void ShootMachinegun()
    {
        GameObject bullet = MachineGunBulletPool.SharedInstance.GetPooledObject();

        if (bullet == null) return;
        
        var trail = bullet.transform.Find("Trail").GetComponent<TrailRenderer>();
        trail.enabled = false;
        
        bullet.transform.position = fireLocation.position;
        bullet.transform.rotation = transform.rotation;
        // bullet.transform.rotation = Quaternion.AngleAxis(90, bullet.transform.right);

        ++shotCount;
        timeSinceLastShot = 0;
        float ranXrot = UnityEngine.Random.Range(-1.0f, 1.0f);
        float ranYrot = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 randDir = Quaternion.Euler(ranXrot, ranYrot,  0) * fireLocation.forward;
        
        BulletController bulletController = bullet.GetComponent<BulletController>();

        //GameObject bullet;
        if (shotCount % tracerSpacing == 0)
        {
            // shoot a tracer
            trail.enabled = true;
            bulletController.canRicochet = true;
            bulletController.isTracer = true;
            // bullet = Instantiate(tracerPrefab, fireLocation.position, transform.rotation);
        }
        else
        {
            // shoot a regular bullet
            bulletController.canRicochet = false;
            bulletController.isTracer = false;
            // bullet = Instantiate(bulletPrefab, fireLocation.position, transform.rotation);
        }
        
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddForce(randDir.normalized * machineGunUpgrades.BulletVelocityModifier, ForceMode.Impulse);
    }
}
