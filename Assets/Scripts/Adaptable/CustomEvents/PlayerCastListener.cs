using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomPlayerCastEvent : UnityEvent<KeyCode>
{

}

public class PlayerCastListener : MonoBehaviour
{
    public PlayerCastEvent Event;
    public CustomPlayerCastEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(KeyCode k)
    {
        Response.Invoke(k);
    }
}
