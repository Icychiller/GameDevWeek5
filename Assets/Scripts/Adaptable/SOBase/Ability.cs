using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability", order = 6)]
public class Ability : ScriptableObject
{
    #if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
    #endif

    public AbilityIndex index;
    public Texture abilityTexture;

    public List<int> Utilise()
    {
        return null;
    }

    public void Reset()
    {
        
    }

    public void Enhance(bool godState)
    {
        
    }

}
