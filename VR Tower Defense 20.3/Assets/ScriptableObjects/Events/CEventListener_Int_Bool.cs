using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CEventListener_Int_Bool : MonoBehaviour
{
    [SerializeField] private CEvent_Int_Bool Event;
    [SerializeField] private UnityEvent_Int_Bool Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int val, bool state)
    {
        Response.Invoke(val, state);
    }
}