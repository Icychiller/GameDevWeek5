using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionMush : MonoBehaviour, ConsumableInterface
{
    public Texture icon;
    public BoolVariable godMode;

    public void consumedBy(GameObject player)
    {
        Debug.Log("Oh Immortality? Okay");
        godMode.SetValue(true);
        StartCoroutine(removeEffect(player));
    }

    IEnumerator removeEffect(GameObject player)
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("Remove Effect? What Remove Effect?");
    }
    public void destroySelf()
    {

    }
}
