using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game/Player/Wall Hit")]
public class WallHitEvent : ScriptableObject
{
    private List<WallHitEventListener> listeners = new List<WallHitEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(WallHitEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(WallHitEventListener listener)
    {
        listeners.Remove(listener);
    }
}