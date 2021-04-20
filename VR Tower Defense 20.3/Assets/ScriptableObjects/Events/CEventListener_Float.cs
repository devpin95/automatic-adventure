using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CEventListener_Float : MonoBehaviour
{
    [SerializeField] private CEvent_Float Event;
    [SerializeField] private UnityEvent_Float Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(float val)
    {
        Response.Invoke(val);
    }
}