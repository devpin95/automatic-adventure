using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButtonController : MonoBehaviour
{
    private float delayBetweenFlips = 0.15f;
    private bool canPress = true;
    public CEvent buttonPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.RIGHT_HAND_TAGNAME) || other.CompareTag(Constants.LEFT_HAND_TAGNAME))
        {
            PushButton();
        }
    }

    private void PushButton()
    {
        if (!canPress) return;

        canPress = false;
        buttonPressed.Raise();
        StartCoroutine(PushDelay());
    }

    IEnumerator PushDelay()
    {
        yield return new WaitForSeconds(delayBetweenFlips);
        canPress = true;
    }
}
