using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MonoBehaviour, ConsumableInterface
{

    private int moveRight = 1;
    private Vector2 velocity;
    public float speed = 5f;
    private Rigidbody2D shroomBody;
    private Collider2D shroomCollider;
    private SpriteRenderer shroomRenderer;
    public IntVariable maxPlayerSpeed;
    private bool collected = false;

    public Texture icon;

    public void consumedBy(GameObject player)
    {
        maxPlayerSpeed.ApplyChange(maxPlayerSpeed.value*2);
        StartCoroutine(removeEffect(player));
    }

    IEnumerator removeEffect(GameObject player)
    {
        yield return new WaitForSeconds(5f);
        maxPlayerSpeed.ApplyChange(maxPlayerSpeed.value/2);
        Debug.Log("Mush Destroyed");
        Destroy(gameObject);
    }

    public void destroySelf()
    {
        Debug.Log("Mush Destroyed");
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        shroomBody = GetComponent<Rigidbody2D>();
        shroomCollider = GetComponent<BoxCollider2D>();
        shroomRenderer = GetComponent<SpriteRenderer>();
        float moveX = Random.Range(-5f, 5f);
        moveRight = (int) Mathf.Floor(moveX/Mathf.Abs(moveX));
        ComputeVelocity();
        shroomBody.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        shroomBody.AddForce( new Vector2(moveX,0), ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if (shroomBody.velocity.magnitude < speed){
            shroomBody.AddForce(velocity, ForceMode2D.Force);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            StopShroom();
            shroomBody.bodyType = RigidbodyType2D.Static;
            collected = true;
            StartCoroutine(bloat());
            // CentralManager.centralManagerInstance.addPowerup(icon, 1, this);
        }
        else{
            foreach(ContactPoint2D contactPoint in col.contacts)
            {
                if(Mathf.Abs(contactPoint.normal.x) > 0)
                {
                    Debug.Log("FLIP THAT SHROOM!");
                    FlipShroom();
                    break;  // Stop calculating for multi-point contacts
                }
            }
        }
    }

    IEnumerator bloat()
    {
        this.shroomCollider.enabled = false;
        this.transform.localScale = new Vector3(1.5f,1.5f,1);
        yield return new WaitForSeconds(0.4f);
        this.transform.localScale = new Vector3(1f,1f,1);
        shroomRenderer.enabled = false;
    }

    void ComputeVelocity()
    {
        velocity = new Vector2(speed * moveRight, 0);
    }
    
    void FlipShroom()
    {
        moveRight *= -1;
        ComputeVelocity();
        shroomBody.AddForce(velocity, ForceMode2D.Impulse);
    }

    void StopShroom()
    {
        speed = 0;
        ComputeVelocity();
    }
}
