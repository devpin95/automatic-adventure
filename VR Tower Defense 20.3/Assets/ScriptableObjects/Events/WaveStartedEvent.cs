using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game/New Wave")]
public class WaveStartedEvent : ScriptableObject
{
    private List<WaveStartedEventListener> listeners = new List<WaveStartedEventListener>();

    public void Raise(int wave)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(wave);
        }
    }

    public void RegisterListener(WaveStartedEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(WaveStartedEventListener listener)
    {
        listeners.Remove(listener);
    }
}