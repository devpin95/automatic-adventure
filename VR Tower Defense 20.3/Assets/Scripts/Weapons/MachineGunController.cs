using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class MachineGunController : MonoBehaviour
{
    public Transform fireLocation;
    public Transform pivotPoint;
    public GameObject tempHand;
    public GameObject tracerPrefab;
    public GameObject bulletPrefab;
    public int tracerSpacing;
    private int shotCount = 0;
    public float bulletSpeedModifier = 100f;
    private Vector3 startingPosition;
    private Vector3 centerPosition;
    private bool selected = false;
    private bool firing = false;
    public float shotDelay = 0.05f;
    private float timeSinceLastShot = 0.0f;
    private XRBaseInteractor _currentInteractor;
    
    // Start is called before the first frame update
    void Start()
    {
        tempHand.SetActive(false);
        startingPosition = pivotPoint.transform.position;
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
            
            Debug.DrawRay(centerPosition, centerToHand, Color.green);

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
                }
            }
        }
    }

    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        selected = true;
        tempHand.SetActive(true);
        _currentInteractor = interactor;
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
}
