using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/CEvent <UpgradeCard>")]
public class CEvent_UpgradeCard : ScriptableObject
{
    private List<CEventListener_UpgradeCard> listeners = new List<CEventListener_UpgradeCard>();

    public void Raise(UpgradeCard card)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(card);
        }
    }

    public void RegisterListener(CEventListener_UpgradeCard listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CEventListener_UpgradeCard listener)
    {
        listeners.Remove(listener);
    }
}