using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CEventListener_UpgradeCard : MonoBehaviour
{
    [SerializeField] private CEvent_UpgradeCard Event;
    [SerializeField] private UnityEvent_UpgradeCard Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(UpgradeCard card)
    {
        Response.Invoke(card);
    }
}
