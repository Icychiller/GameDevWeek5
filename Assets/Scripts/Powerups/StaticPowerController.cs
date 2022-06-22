using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPowerController : MonoBehaviour
{
    public Ability stats;
    public CustomAbilityEvent onCollected;

    private Collider2D flowerCollider;
    private SpriteRenderer flowerRenderer;
    private bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        flowerCollider = GetComponent<BoxCollider2D>();
        flowerRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {

    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            collected = true;
            onCollected.Invoke(stats);
            StartCoroutine(bloat());
        }
    }

    IEnumerator bloat()
    {
        this.flowerCollider.enabled = false;
        this.transform.localScale = new Vector3(1.5f,1.5f,1);
        yield return new WaitForSeconds(0.4f);
        this.transform.localScale = new Vector3(1f,1f,1);
        flowerRenderer.enabled = false;
        Destroy(this.gameObject);
    }
}
