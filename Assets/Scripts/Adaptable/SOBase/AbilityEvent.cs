using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEvent", menuName = "ScriptableObjects/AbilityEvent", order = 8)]
public class AbilityEvent : ScriptableObject
{
    private readonly List<AbilityEventListener> eventListeners = new List<AbilityEventListener>();

    public void Raise(Ability p)
    {
        for(int i = eventListeners.Count - 1; i>=0; i--)
        {
            eventListeners[i].OnEventRaised(p);
        }
    }

    public void RegisterListener(AbilityEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(AbilityEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}
