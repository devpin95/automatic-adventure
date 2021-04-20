using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent <Int>")]
public class CEvent_Int : ScriptableObject
{
    private List<CEventListener_Int> listeners = new List<CEventListener_Int>();

    public void Raise(int val)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(val);
        }
    }

    public void RegisterListener(CEventListener_Int listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener_Int listener)
    {
        listeners.Remove(listener);
    }
}