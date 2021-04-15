using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerSelectManager : MonoBehaviour
{
    public GameObject handModel;
    
    [SerializeField] private Devices _devices;
    private string _tag;
    private XRDirectInteractor _directInteractor;
    private Vector3 _handModelStartingScale;

    private void Start()
    {
        _tag = gameObject.tag;
        _directInteractor = GetComponent<XRDirectInteractor>();
        _handModelStartingScale = handModel.transform.localScale;
    }

    public void OnSelectEntered(XRBaseInteractable interactable)
    {
        _devices.SetSelectStateByTagname(_tag, true);

        if (_directInteractor.hideControllerOnSelect)
        {
            handModel.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void OnSelectedExited(XRBaseInteractable interactable)
    {
        _devices.SetSelectStateByTagname(_tag, false);

        if (_directInteractor.hideControllerOnSelect)
        {
            handModel.transform.localScale = _handModelStartingScale;
        }
    }
}
