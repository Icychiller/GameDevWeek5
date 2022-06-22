using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/FloatVariable", order = 5)]
public class FloatVariable : ScriptableObject
{
    #if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "Just another Float Variable";
    #endif
    
    private float _value = 0;
    public float value
    {
        get
        {
            return _value;
        }
    }

    public void SetValue(float value)
    {
        _value = value;
    }

    public void SetValue(FloatVariable value)
    {
        _value = value._value;
    }

    public void ApplyChange(float amount)
    {
        _value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        _value += amount._value;
    }
}
