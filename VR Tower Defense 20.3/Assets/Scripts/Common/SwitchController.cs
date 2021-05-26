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
    public CEvent_Bool flip;

    public Vector3 onStateAngle;
    public Vector3 offStateAngle;

    private float delayBetweenFlips = 0.15f;
    private bool canFlip = true;
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = startState;
        if (currentState) transform.rotation = Quaternion.Euler(onStateAngle);
        else transform.rotation = Quaternion.Euler(offStateAngle);
        // FlipSwitch("", false);
        // flip?.Raise(currentState);
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

    private void FlipSwitch(string tag, bool raise = true)
    {
        if (!canFlip) return;

        currentState = !currentState;

        if (currentState) transform.rotation = Quaternion.Euler(onStateAngle);
        else transform.rotation = Quaternion.Euler(offStateAngle);

        if (raise)
        {
            flip.Raise(currentState);
            if (hapticsManager) hapticsManager.RequestOneOffRumble(tag, 0.1f, 0.1f);
            canFlip = false;
            StartCoroutine(SwitchDelay());
        }
    }

    IEnumerator SwitchDelay()
    {
        yield return new WaitForSeconds(delayBetweenFlips);
        canFlip = true;
    }
}
