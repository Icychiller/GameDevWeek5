using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour, EnemyInterface
{

	public float speed = 5f;

	public Animator animator;
	public Rigidbody2D rb;
	public GameObject progessStar;
	public IntVariable gameScore;
	public FloatVariable hpPercent;
	public BoxCollider2D bc;
	public AudioSource audioSource;

	public UnityEvent onPlayerDeath;

	Vector2 move;

	SpriteRenderer sr;

	private int maxHp = 30;
	private int hp = 30;

	public void takeDamage()
	{
		hp--;
		audioSource.PlayOneShot(audioSource.clip);
		hpPercent.SetValue(((float)hp)/maxHp);
	}

	public void takeDamage(int damage)
	{
		hp -= damage;
		audioSource.PlayOneShot(audioSource.clip);
		hpPercent.SetValue(((float)hp)/maxHp);
		Debug.Log("New HP: "+hp.ToString());
	}

    // Start is called before the first frame update
    void Start()
    {
		sr = GetComponent<SpriteRenderer>();
		bc = GetComponent<BoxCollider2D>();
		audioSource = GetComponent<AudioSource>();
    }

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			if ((bc.bounds.center.y) <= col.collider.bounds.min.y)
			{
				takeDamage(2);
				knockback();
			}
			else
			{
				onPlayerDeath.Invoke();
			}
		}
		
	}

	public void knockback()
	{
		Debug.Log("knockback engaged");
		rb.AddForce(new Vector2(Random.Range(-5f, 5f), 0)*2,ForceMode2D.Impulse);
	}

    // Update is called once per frame
    void Update()
    {
		move.x = Input.GetAxisRaw("Horizontal");

		animator.SetFloat("Speed", Mathf.Abs(move.x));

		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Input.GetKeyDown("q") || Input.GetKeyDown("e"))
		{
			animator.SetTrigger("Attack");
		}

		if (hp <= 0)
		{
			gameScore.ApplyChange(1000);
			Instantiate(progessStar,new Vector3(this.transform.position.x,this.transform.position.y+2,this.transform.position.z),Quaternion.identity);
			Destroy(gameObject);
			
		}
    }

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);

		if (move.x < 0f)
		{
			sr.flipX = true;
		} else if (move.x > 0f)
		{
			sr.flipX = false;
		}
	}
}
