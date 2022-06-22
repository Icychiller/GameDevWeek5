using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomAbilityEvent : UnityEvent<Ability>
{

}

public class AbilityEventListener : MonoBehaviour
{
    public AbilityEvent Event;
    public CustomAbilityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Ability p)
    {
        Response.Invoke(p);
    }
    
}
