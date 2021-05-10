using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Devices devices;
    public HapticsManager hapticsManager;
    
    public bool startState = false;
    private bool currentState;
    public string triggerTagName;
    public CEvent_Bool flip;

    public float onStateAngle = 30;
    public float offStateAngle = -30;

    private float delayBetweenFlips = 1;
    private bool canFlip = true;
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = startState;
        
        Vector3 angles = transform.rotation.eulerAngles;
        
        if (currentState) angles.x = onStateAngle;
        else angles.x = offStateAngle;
        
        transform.rotation = Quaternion.Euler(angles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Right Hand Interactor") || other.CompareTag("Left Hand Interactor"))
        {
            FlipSwitch(other.tag);
        }
    }

    private void FlipSwitch(string tag)
    {
        if (!canFlip) return;
        
        hapticsManager.RequestOneOffRumble(tag, 0.1f, 0.1f);
        currentState = !currentState;

        Vector3 angles = transform.rotation.eulerAngles;
        
        if (currentState) angles.x = onStateAngle;
        else angles.x = offStateAngle;
        
        transform.rotation = Quaternion.Euler(angles);
        
        flip.Raise(currentState);

        canFlip = false;
        StartCoroutine(SwitchDelay());
    }

    IEnumerator SwitchDelay()
    {
        yield return new WaitForSeconds(delayBetweenFlips);
        canFlip = true;
    }
}
