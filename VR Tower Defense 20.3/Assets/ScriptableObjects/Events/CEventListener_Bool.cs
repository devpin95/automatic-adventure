using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEventListener_Bool : MonoBehaviour
{
    [SerializeField] private CEvent_Bool Event;
    [SerializeField] private UnityEvent_Bool Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(bool state)
    {
        Response.Invoke(state);
    }
}
