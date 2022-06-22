using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerup", menuName = "ScriptableObjects/Powerup", order = 6)]
public class Powerup : ScriptableObject
{
    #if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
    #endif

    public PowerupIndex index;
    public Texture powerupTexture;

    public int absoluteSpeedBooster;
    public int absoluteJumpBooster;
    public bool godMode;

    public int defaultDuration;

    public List<int> Utilise()
    {
        return new List<int> {absoluteSpeedBooster, absoluteJumpBooster};
    }

    public void Reset()
    {
        absoluteJumpBooster = 0;
        absoluteSpeedBooster = 0;
        godMode = false;
    }

    public void Enhance(int speedBooster, int jumpBooster, bool godState)
    {
        absoluteSpeedBooster += speedBooster;
        absoluteJumpBooster += jumpBooster;
        godMode = godState;
    }

}
