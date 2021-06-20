using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CEventListener_BountyTrackedData : MonoBehaviour
{
    [SerializeField] private CEvent_BountyTrackedData Event;
    [SerializeField] private UnityEvent_BountyTrackedData Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(BountyTrackedData data)
    {
        Response.Invoke(data);
    }
}