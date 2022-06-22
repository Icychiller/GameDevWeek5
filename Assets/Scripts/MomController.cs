using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MomController : MonoBehaviour, EnemyInterface
{
    public UnityEvent spawnRequested;
    public UnityEvent onDeath;
    public UnityEvent onPlayerDeath;
    public GameConstants gameConstants;
    public BoolVariable playOnGround;
    public FloatVariable hpPercent;
    public IntVariable playerScore;
    private int maxHp = 100;
    private int hp = 100;
    private Animator mushAnim;
    private AudioSource mushAudio;
    public Animator stompAnim;
    public GameObject enemyPrefab;

    private bool giveAway = true;
    private bool animPlaying = false;

    private bool stompCooldown = false;
    private bool spawnCooldown = false;
    private bool naturalAggro = false;
    
    // Start is called before the first frame update
    void Start()
    {
        mushAnim = GetComponent<Animator>();
        mushAudio = GetComponent<AudioSource>();
        StartCoroutine( startCooldownAggro(gameConstants.aggroTimer));
        StartCoroutine( startCooldownSpawn(gameConstants.spawnCooldown));
    }

    public void takeDamage()
    {
        hp--;
        mushAudio.PlayOneShot(mushAudio.clip);
        hpPercent.SetValue(((float)hp)/maxHp);
        if (!stompCooldown)
        {
            triggerStomp();
            StartCoroutine( startCooldownStomp(gameConstants.stompCooldown));
        }
    }

    public void checkStomp()
    {
        if(playOnGround.value)
        {
            onPlayerDeath.Invoke();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!naturalAggro)
        {
            if (!stompCooldown)
            {
                triggerStomp();
                StartCoroutine( startCooldownStomp(gameConstants.stompCooldown));
            }
            StartCoroutine(startCooldownAggro(gameConstants.aggroTimer));
        }
        if(hp <= 0)
        {
            onDeath.Invoke();
            playerScore.ApplyChange(10000);
            Destroy(gameObject);
        }
    }

    
    void triggerStomp()
    {
        mushAnim.SetTrigger("Stomp");
        stompAnim.SetTrigger("Stomp");
        
    }

    IEnumerator startCooldownAggro(float cooldown)
    {
        naturalAggro = true;
        yield return new WaitForSeconds(cooldown);
        naturalAggro = false;
    }
    IEnumerator startCooldownStomp(float cooldown)
    {
        stompCooldown = true;
        animPlaying = true;
        
        yield return new WaitUntil(()=> mushAnim.GetCurrentAnimatorStateInfo(0).IsName("Base.MushIdle"));
        animPlaying = false;
        if(!giveAway)
            checkStomp();
        yield return new WaitForSeconds(cooldown);
        //checkStomp();
        stompCooldown = false;
        giveAway = false;
    }
    IEnumerator startCooldownSpawn(float cooldown)
    {
        spawnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        spawnCooldown = false;
    }

    IEnumerator TestMushStomp()
    {
        animPlaying = true;
        mushAnim.SetTrigger("Stomp");
        stompAnim.SetTrigger("Stomp");
        yield return new WaitForSeconds(2f);
        animPlaying = false;
    }
}
