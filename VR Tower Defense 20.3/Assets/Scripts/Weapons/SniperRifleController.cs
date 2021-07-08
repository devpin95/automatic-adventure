using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SniperRifleController : MonoBehaviour
{
    [Header("Firing")] 
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireDelay = 1.5f;
    public float fireCounter = 0;
    
    [Header("Interaction Variables")]
    public float majorRotationSpeed = 25;
    public float minorRotationSpeed = 25;
    public float majorRotationSlowSpeed = 15;
    public float minorRotationSlowSpeed = 15;
    public bool rotationSlowed = false;
    
    [Header("Camera Settings")] 
    public float minZoom;
    public float maxZoom;

    [Header("Action Map Event")] 
    public CEvent_Int switchActionMap;
    public InputActionReference sniperAimAction;
    public InputActionReference sniperToggleAimSpeed;
    
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private TextMeshProUGUI _rangeText;
    
    // Start is called before the first frame update
    void Start()
    {
        _cinemachineVirtualCamera = transform.Find("Sniper Rifle Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        _cinemachineVirtualCamera.m_Lens.FieldOfView = maxZoom;
        
        sniperToggleAimSpeed.action.performed += ToggleRotationSpeed;
    }

    private void OnDestroy()
    {
        sniperToggleAimSpeed.action.performed -= ToggleRotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // CheckToggleButton();
        MoveCamera();
        fireCounter += Time.deltaTime;
    }

    public void Fire()
    {
        if (fireCounter >= fireDelay)
        {
            var bulletInstance = Instantiate(bulletPrefab, firePoint.transform.position, bulletPrefab.transform.rotation);
            
            bulletInstance.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * 75, ForceMode.Impulse);
            
            fireCounter = 0;   
        }
    }
    
    public void CameraFeedZoomChange(float zoom)
    {

        float clampedZoom = Mathf.Lerp(maxZoom, minZoom, zoom);
        _cinemachineVirtualCamera.m_Lens.FieldOfView = clampedZoom;
    }

    public void WeaponSelected(bool selected)
    {
        if (selected)
        {
            // the weapon is now being controlled
            switchActionMap.Raise((int)PlayerActionStateManager.ActionMap.HeavyWeaponSniperRifle);
        }
        else
        {
            // the weapon is no longer being controlled
            switchActionMap.Raise((int)PlayerActionStateManager.ActionMap.Freemovement);
        }
    }
    
    private void GetGUIObjects()
    {
        HeavyWeaponGuiSearcher guiSearcher = GetComponent<HeavyWeapon>().gui.GetComponent<HeavyWeaponGuiSearcher>();
        _rangeText = guiSearcher.FindSingleGUIElementByName("Distance").GetComponent<TextMeshProUGUI>();
    }

    private void ToggleRotationSpeed(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            rotationSlowed = !rotationSlowed;
        }
    }

    private void MoveCamera()
    {
        Vector2 joystickVal = sniperAimAction.action.ReadValue<Vector2>();
        float angle = joystickVal.x * (rotationSlowed ? majorRotationSlowSpeed : majorRotationSpeed) * Time.deltaTime;
        transform.RotateAround(transform.position, transform.up, angle);
                
        angle = -joystickVal.y * (rotationSlowed ? minorRotationSlowSpeed : minorRotationSpeed) * Time.deltaTime;
        transform.RotateAround(transform.position, transform.right, angle);

        // cancel out any rotation on the z, there's probably a better way to do this
        transform.Rotate(0, 0, -transform.eulerAngles.z);
    }
}
