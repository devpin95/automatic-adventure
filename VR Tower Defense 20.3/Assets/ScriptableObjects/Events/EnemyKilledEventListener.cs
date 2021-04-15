using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyKilledEventListener : MonoBehaviour
{
    [SerializeField] private EnemyKilledEvent Event;
    [SerializeField] private UE_EnemyKilled Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(float value)
    {
        Response.Invoke(value);
    }
}