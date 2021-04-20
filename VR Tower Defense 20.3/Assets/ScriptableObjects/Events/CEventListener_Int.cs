using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CEventListener_Int : MonoBehaviour
{
    [SerializeField] private CEvent_Int Event;
    [SerializeField] private UnityEvent_Int Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int val)
    {
        Response.Invoke(val);
    }
}