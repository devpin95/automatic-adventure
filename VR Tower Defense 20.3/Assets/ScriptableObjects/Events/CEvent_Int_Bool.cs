using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent <Int, Bool>")]
public class CEvent_Int_Bool : ScriptableObject
{
    private List<CEventListener_Int_Bool> listeners = new List<CEventListener_Int_Bool>();

    public void Raise(int val, bool state)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(val, state);
        }
    }

    public void RegisterListener(CEventListener_Int_Bool listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener_Int_Bool listener)
    {
        listeners.Remove(listener);
    }
}