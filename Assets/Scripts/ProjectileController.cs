using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileController : MonoBehaviour
{
    public BoolVariable marioFaceRight;
    private bool currentFaceRight;
    private Rigidbody2D fireBody;
    private int life = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentFaceRight = marioFaceRight.value;
        fireBody = GetComponent<Rigidbody2D>();
        fireBody.AddForce(Vector2.up*5,ForceMode2D.Impulse);
        xLaunch();
    }

    void xLaunch()
    {
        if(currentFaceRight)
        {
            fireBody.AddForce(Vector2.right*5,ForceMode2D.Impulse);
        }
        else
        {
            fireBody.AddForce(Vector2.left*5, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {

            EnemyInterface enemy = col.gameObject.GetComponent<EnemyInterface>();
            enemy.takeDamage();
            Destroy(gameObject);
        }
        
        foreach(ContactPoint2D contactPoint in col.contacts)
        {
            //Debug.Log(contactPoint.normal);
            if(contactPoint.normal.y > 0)
            {
                //Debug.Log("Mario is On something. It might be shrooms");
                life--;
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {

            EnemyInterface enemy = col.gameObject.GetComponent<EnemyInterface>();
            enemy.takeDamage();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
