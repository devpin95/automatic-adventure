using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game/Enemy/Killed")]
public class EnemyKilledEvent : ScriptableObject
{
    private List<EnemyKilledEventListener> listeners = new List<EnemyKilledEventListener>();

    public void Raise(float value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(EnemyKilledEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(EnemyKilledEventListener listener)
    {
        listeners.Remove(listener);
    }
}