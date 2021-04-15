using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveStartedEventListener : MonoBehaviour
{
    [SerializeField] private WaveStartedEvent Event;
    [SerializeField] private UE_WaveStarted Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int wave)
    {
        Response.Invoke(wave);
    }
}