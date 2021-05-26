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

    public void AimWeapon(XRBaseInteractor currentInteractor, Vector3 center)
    {
        Vector3 handPos = currentInteractor.transform.position;
        Vector3 centerToHand = handPos - center;
        transform.rotation = Quaternion.LookRotation(-centerToHand);
        // Vector3 angles = transform.localRotation.eulerAngles;
        // transform.localRotation = Quaternion.Euler(angles.x, angles.y + 116.293f, angles.z);
        
        transform.Rotate(0, -90, -transform.eulerAngles.z);
        
        Debug.DrawLine(handPos, center);
    }
}
