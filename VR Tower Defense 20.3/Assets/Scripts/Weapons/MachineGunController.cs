using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MachineGunController : MonoBehaviour
{
    public Transform fireLocation;
    public Transform pivotPoint;
    public GameObject tempHand;
    private Vector3 startingPosition;
    private Vector3 centerPosition;
    private bool selected;
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
        }
    }

    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        tempHand.SetActive(true);
        // interactor.gameObject.SetActive(false);
        selected = true;
        _currentInteractor = interactor;
        print("Select Enter");
    }

    public void OnSelectExit(XRBaseInteractor interactor)
    {
        selected = false;
        tempHand.SetActive(false);
        // interactor.gameObject.SetActive(true);
        _currentInteractor = null;
        print("Select Exited");
    }
}
