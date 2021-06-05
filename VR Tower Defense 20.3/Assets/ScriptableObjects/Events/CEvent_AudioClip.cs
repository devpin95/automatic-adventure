using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent <AudioClip>")]
public class CEvent_AudioClip : ScriptableObject
{
    private List<CEventListener_AudioClip> listeners = new List<CEventListener_AudioClip>();

    public void Raise(AudioClip clip)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(clip);
        }
    }

    public void RegisterListener(CEventListener_AudioClip listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener_AudioClip listener)
    {
        listeners.Remove(listener);
    }
}