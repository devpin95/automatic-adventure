using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HeavyWeaponController : MonoBehaviour
{
    public GameObject placeholderHand;

    public CEvent heavyWeaponFireEvent;
    public CEvent_Bool heavyWeaponSelectEvent;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnMainControllerSelected(XRBaseInteractor interactor)
    {
        heavyWeaponSelectEvent.Raise(true);
        Debug.Log("Taking control of heavy weapon");
        placeholderHand.SetActive(true);
    }

    public void OnMainControllerDeselected(XRBaseInteractor interactor)
    {
        heavyWeaponSelectEvent.Raise(false);
        Debug.Log("Relinquishing control of heavy weapon");
        placeholderHand.SetActive(false);
    }

    public void OnMainControllerActivated(XRBaseInteractor interactor)
    {
        Debug.Log("Heavy weapon main trigger activated");
        heavyWeaponFireEvent.Raise();
    }
}
