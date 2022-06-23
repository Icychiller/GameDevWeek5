using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    public GameConstants gameConstants;
    private bool deathTime = false;
    private bool allClear = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(floatTillDeath());
        StartCoroutine(floatingText());
    }

    IEnumerator floatTillDeath()
    {
        yield return new WaitForSeconds(gameConstants.floatingTextLifetime);
        deathTime = true;
    }
    IEnumerator floatingText()
    {
        while (!deathTime)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+gameConstants.floatingSpeed,this.transform.position.z);
            yield return null;
        }
        allClear = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(allClear)
        {
            Destroy(gameObject);
        }
    }
}
