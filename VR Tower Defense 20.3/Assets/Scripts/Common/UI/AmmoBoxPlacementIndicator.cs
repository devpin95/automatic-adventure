using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class AmmoBoxPlacementIndicator : MonoBehaviour
{

    public Transform target;
    [FormerlySerializedAs("indicatorObject")] public GameObject indicatorPrefab;
    private GameObject _indicatorInstance;
    private int _callCount = 0;

    private void Start()
    {
        _indicatorInstance = Instantiate(indicatorPrefab, target.position, target.rotation);
    }

    private void Update()
    {
        _indicatorInstance.transform.position = target.position;
        _indicatorInstance.transform.rotation = target.rotation;
    }

    public void ShowIndicator(XRBaseInteractable interactable)
    {
        ++_callCount;
        if (_callCount == 1) return;
        _indicatorInstance.SetActive(true);
    }

    public void HideIndicator(XRBaseInteractable interactable)
    {
        _indicatorInstance.SetActive(false);
    }

}
