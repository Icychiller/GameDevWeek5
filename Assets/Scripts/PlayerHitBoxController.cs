using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBoxController : MonoBehaviour
{
    public GameConstants gameConstants;
    public BoolVariable mudaState;
    private bool damageInterval = false;

    private List<Collider2D> damageList = new List<Collider2D>();
    // Start is called before the first frame update

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy") && mudaState)
        {
            if (!damageList.Contains(col))
                damageList.Add(col);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        damageList.Remove(col);
    }

    // void OnTriggerStay2D(Collider2D col)
    // {
    //     Debug.Log(mudaState.value.ToString() + damageInterval.ToString());
    //     if(col.gameObject.CompareTag("Enemy") && mudaState && !damageInterval)
    //     {
    //         Debug.Log("Entry");
    //         col.gameObject.GetComponent<EnemyInterface>().takeDamage();
    //         Debug.Log("Enemy Found! "+col.gameObject.name);
    //         StartCoroutine(onCooldown());
    //     }
    // }

    IEnumerator onCooldown()
    {
        damageInterval = true;
        yield return new WaitForSeconds(0.1f);
        damageInterval = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!mudaState.value)
        {
            damageList.Clear();
        }
        else if (!damageInterval)
        {
            foreach(Collider2D col in damageList)
            {
                col.gameObject.GetComponent<EnemyInterface>().takeDamage();
                StartCoroutine(onCooldown());
            }
        }
    }
}
