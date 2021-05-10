using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent <Bool>")]
public class CEvent_Bool : ScriptableObject
{
    private List<CEventListener_Bool> listeners = new List<CEventListener_Bool>();

    public void Raise(bool state)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(state);
        }
    }

    public void RegisterListener(CEventListener_Bool listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener_Bool listener)
    {
        listeners.Remove(listener);
    }
}
