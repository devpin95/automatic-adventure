using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent <Float>")]
public class CEvent_Float : ScriptableObject
{
    private List<CEventListener_Float> listeners = new List<CEventListener_Float>();

    public void Raise(float val)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(val);
        }
    }

    public void RegisterListener(CEventListener_Float listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener_Float listener)
    {
        listeners.Remove(listener);
    }
}