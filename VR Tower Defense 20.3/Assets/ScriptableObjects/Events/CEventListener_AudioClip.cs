using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEventListener_AudioClip : MonoBehaviour
{
    [SerializeField] private CEvent_AudioClip Event;
    [SerializeField] private UnityEvent_AudioClip Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(AudioClip clip)
    {
        Response.Invoke(clip);
    }
}