using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "ScriptableObjects/BoolVariable", order = 4)]
public class BoolVariable : ScriptableObject
{
    #if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "Just another Float Variable";
    #endif
    
    private bool _value = false;
    public bool value
    {
        get
        {
            return _value;
        }
    }

    public void SetValue(bool value)
    {
        _value = value;
    }

    public void SetValue(BoolVariable value)
    {
        _value = value._value;
    }

    public void Inverse()
    {
        _value = !_value;
    }

}
