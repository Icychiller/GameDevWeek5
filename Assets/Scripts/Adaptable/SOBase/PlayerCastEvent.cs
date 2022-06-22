using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCastEvent", menuName = "ScriptableObjects/PlayerCastEvent", order = 9)]
public class PlayerCastEvent : ScriptableObject
{
    private readonly List<PlayerCastListener> eventListeners = new List<PlayerCastListener>();

    public void Raise(KeyCode k)
    {
        for(int i = eventListeners.Count - 1; i>=0; i--)
        {
            eventListeners[i].OnEventRaised(k);
        }
    }

    public void RegisterListener(PlayerCastListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(PlayerCastListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}
