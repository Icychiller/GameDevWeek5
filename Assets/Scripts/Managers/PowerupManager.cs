using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    REDMUSHROOM = 0,
    GREENMUSHROOM = 1,
    SPAGHETTI = 2
}
public class PowerupManager : MonoBehaviour
{
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public BoolVariable marioGodMode;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    private List<KeyCode> validKeys = new List<KeyCode>{KeyCode.Z, KeyCode.X, KeyCode.C};

    void Start()
    {
        if (!powerupInventory.gameStarted)
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            for (int i = 0; i < powerupInventory.Items.Count; i++)
            {
                Powerup p = powerupInventory.Get(i);
                if (p != null)
                {
                    AddPowerupUI(i, p.powerupTexture);
                }
                else
                {
                    RemovePowerupUI(i);
                }
            }
        }
    }

    public void resetPowerup()
    {
        Debug.Log("UI Reset Called");
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }

    void AddPowerupUI(int index, Texture t)
    {
        powerupIcons[index].GetComponent<RawImage>().texture = t;
        powerupIcons[index].SetActive(true);
    }

    void RemovePowerupUI(int index)
    {
        powerupIcons[index].SetActive(false);
    }

    public void AddPowerup(Powerup p)
    {
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupTexture);
    }

    public void ApplyPowerup(KeyCode keyCode)
    {
        int keyIndex = validKeys.IndexOf(keyCode);
        if (keyIndex != -1)
        {
            Powerup powerupEffect = powerupInventory.Get(keyIndex);
            if (powerupEffect != null)
            {
                StartCoroutine(EnhanceMario(powerupEffect));
                powerupInventory.Remove(keyIndex);
                RemovePowerupUI(keyIndex);
            }
            
        }
    }

    IEnumerator EnhanceMario(Powerup p)
    {
        marioGodMode.SetValue(p.godMode);
        marioJumpSpeed.ApplyChange(p.absoluteJumpBooster);
        marioMaxSpeed.ApplyChange(p.absoluteSpeedBooster);
        yield return new WaitForSeconds(p.defaultDuration);
        marioGodMode.SetValue(false);
        marioJumpSpeed.ApplyChange(-p.absoluteJumpBooster);
        marioMaxSpeed.ApplyChange(-p.absoluteSpeedBooster);
    }

    public void OnApplicationQuit()
    {
        ResetValues();
    }

    public void ResetValues()
    {
        resetPowerup();
        powerupInventory.Clear();
    }

}