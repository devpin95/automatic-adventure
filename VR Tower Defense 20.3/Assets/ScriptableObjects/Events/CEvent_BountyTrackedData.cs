using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent <BountyTrackedData>")]
public class CEvent_BountyTrackedData : ScriptableObject
{
    private List<CEventListener_BountyTrackedData> listeners = new List<CEventListener_BountyTrackedData>();

    public void Raise(BountyTrackedData val)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(val);
        }
    }

    public void RegisterListener(CEventListener_BountyTrackedData listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener_BountyTrackedData listener)
    {
        listeners.Remove(listener);
    }
}