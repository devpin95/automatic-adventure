using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent")]
public class CEvent : ScriptableObject
{
    private List<CEventListener> listeners = new List<CEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(CEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener listener)
    {
        listeners.Remove(listener);
    }
}