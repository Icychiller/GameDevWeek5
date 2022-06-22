using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityIndex
{
    Fireball = 0,
    Mudamuda = 1
}
public class AbilityManager : MonoBehaviour
{
    public BoolVariable fireball;
    public BoolVariable muda;
    public AbilityInventory abilityInventory;
    public List<GameObject> abilityIcons;

    private List<BoolVariable> abilityList;

    void Start()
    {
        abilityList = new List<BoolVariable>{fireball, muda};
        if (!abilityInventory.gameStarted)
        {
            abilityInventory.gameStarted = true;
            abilityInventory.Setup(abilityIcons.Count);
            resetAbility();
        }
        else
        {
            for (int i = 0; i < abilityInventory.Items.Count; i++)
            {
                Ability p = abilityInventory.Get(i);
                if (p != null)
                {
                    AddAbilityUI(i, p.abilityTexture);
                }
                else
                {
                    RemoveAbilityUI(i);
                }
            }
        }
    }

    public void setListFalse()
    {
        foreach(BoolVariable bv in abilityList)
        {
            bv.SetValue(false);
        }
    }

    public void resetAbility()
    {
        Debug.Log("UI Reset Called");
        for (int i = 0; i < abilityIcons.Count; i++)
        {
            abilityIcons[i].SetActive(false);
        }
    }

    void AddAbilityUI(int index, Texture t)
    {
        abilityIcons[index].GetComponent<RawImage>().texture = t;
        abilityIcons[index].SetActive(true);
    }

    void RemoveAbilityUI(int index)
    {
        abilityIcons[index].SetActive(false);
    }

    public void AddAbility(Ability p)
    {
        abilityInventory.Add(p, (int)p.index);
        AddAbilityUI((int)p.index, p.abilityTexture);
        abilityList[(int)p.index].SetValue(true);
    }

    public void OnApplicationQuit()
    {
        ResetValues();
    }

    public void ResetValues()
    {
        resetAbility();
        abilityInventory.Clear();
        setListFalse();
    }

}