using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Devices/Connected")]
public class DevicesReadyEvent : ScriptableObject
{
    private List<DevicesReadyEventListener> listeners = new List<DevicesReadyEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(DevicesReadyEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(DevicesReadyEventListener listener)
    {
        listeners.Remove(listener);
    }
}
