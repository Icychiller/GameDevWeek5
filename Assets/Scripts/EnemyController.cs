using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour, EnemyInterface
{
    public UnityEvent onPlayerDeath;
    public UnityEvent onEnemyDeath;

    public GameConstants gameConstants;
    public BoolVariable playerGodMode;
    public FloatVariable playerXPos;

    private float originalX;
    private float maxLeft = -5f;
    private float maxRight = 5f;
    private float speed = 4f;
    private int moveRight = -1;
    private float speedMultiplier = 1f;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    private BoxCollider2D enemyCollider;
    private SpriteRenderer enemySprite;

    public BoxCollider2D patrolRegion;
    public bool detectPlayer = false;
    
    public int id = -1;
    private float originalY;
    private float originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
        originalScale = transform.localScale.y;
        enemyBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemySprite = GetComponent<SpriteRenderer>();

        originalX = transform.position.x;
        // Get Patrol Region Limits
        maxLeft = patrolRegion.bounds.min.x + enemyCollider.size.x/2;
        maxRight = patrolRegion.bounds.max.x - enemyCollider.size.x/2;
        ComputeVelocity();
        // GameManager.OnPlayerDeath += EnemyRejoice;
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight)*speed, 0 );
    }

    void MoveGoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime * speedMultiplier);
    }

    void FlipGoomba()
    {
        moveRight *= -1;
        enemySprite.flipX = (moveRight != -1);
        ComputeVelocity();
        MoveGoomba();
    }

    // Update is called once per frame
    void Update()
    {
        if ((enemyBody.position.x < maxLeft && moveRight<0) || (enemyBody.position.x > maxRight && moveRight>0))
        {
            FlipGoomba();
        }
        else {
            MoveGoomba();
        }

        if(detectPlayer)
        {
            if(((playerXPos.value - enemyBody.position.x) * moveRight) < 0)
            {
                // Debug.Log("Detect Mario and Wrong Way");
                FlipGoomba();
            }
            speedMultiplier = 1.5f;
        }
        else
        {
            speedMultiplier = 1f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check for collision within 50% margin
            if ((enemyCollider.bounds.center.y) <= other.bounds.min.y || playerGodMode.value)
            {
                KillSelf();
            }
            else
            {
                // CentralManager.centralManagerInstance.damagePlayer();
                onPlayerDeath.Invoke();
            }
        }
    }

    public void SetID(int id)
    {
        this.id = id;
    }

    public void takeDamage()
    {
        KillSelf();
    }

    private void KillSelf()
    {
        Debug.Log("Invoke Death");
        onEnemyDeath.Invoke();
        //CentralManager.centralManagerInstance.increaseScore();
        //CentralManager.centralManagerInstance.deadEnemy(this.id);
        StartCoroutine(flatten());
    }
    IEnumerator flatten()
    {
        int steps = gameConstants.flattenSteps;
        float stepper = 1f/(float)steps;

        for (int i = 0; i < steps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);

            this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
            yield return null;
        }
        transform.parent.gameObject.SetActive(false);
        this.transform.localScale = new Vector3(this.transform.localScale.x,originalScale,this.transform.localScale.z);
        this.transform.position = new Vector3(this.transform.position.x,originalY,this.transform.position.z);
        yield break;
    }

    public void PlayerDeathResponse()
    {
        this.speed = 0;
        this.velocity = Vector2.zero;
        if (this.gameObject.activeInHierarchy)
            StartCoroutine(dance());
    }

    IEnumerator dance()
    {
        while(true)
        {
            enemySprite.flipX = !enemySprite.flipX;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
