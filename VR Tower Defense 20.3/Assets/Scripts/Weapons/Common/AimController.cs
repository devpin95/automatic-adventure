using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AimController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AimWeapon(XRBaseInteractor _currentInteractor, Vector3 center)
    {
        Vector3 handPos = _currentInteractor.transform.position;
        Vector3 centerToHand = handPos - center;
        transform.rotation = Quaternion.LookRotation(-centerToHand);
        Vector3 angles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(angles.x + 90, angles.y, angles.z);
    }
}
