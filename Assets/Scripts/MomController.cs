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
    public CheapFloatingTextGenerator textGen;

    private bool giveAway = true;       // Defend against 1 attack
    private bool animPlaying = false;

    private bool stompCooldown = false;
    private bool spawnCooldown = false;
    private bool naturalAggro = false;
    
    // Start is called before the first frame update
    void Start()
    {
        mushAnim = GetComponent<Animator>();
        mushAudio = GetComponent<AudioSource>();
    }

    public void takeDamage()
    {
        hp--;
        mushAudio.PlayOneShot(mushAudio.clip);
        hpPercent.SetValue(((float)hp)/maxHp);
        textGen.GenerateText();
        if (!stompCooldown)
        {
            stompCooldown = true;
            StartCoroutine(startCooldownStomp(gameConstants.stompCooldown));
        }
    }

    public void checkStomp()
    {
        Debug.Log("Entered Check");
        if(playOnGround.value && !giveAway)
        {
            onPlayerDeath.Invoke();
        } else if (giveAway)
        {
            giveAway = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stompCooldown)
        {
            Debug.Log("Enter Loop");
            stompCooldown = true;
            StartCoroutine(startCooldownStomp(gameConstants.stompCooldown));
        }
        if (hp <= 0)
        {
            onDeath.Invoke();
            playerScore.ApplyChange(99999);
            Destroy(gameObject);
        }
    }

    void triggerStomp()
    {
        mushAnim.SetTrigger("Stomp");
        stompAnim.SetTrigger("Stomp");
        Debug.Log("Stomp Triggered");
    }

    IEnumerator startCooldownAggro(float cooldown)
    {
        naturalAggro = true;
        yield return new WaitForSeconds(cooldown);
        naturalAggro = false;
    }
    IEnumerator startCooldownStomp(float cooldown)
    {
        Debug.Log ( "Entered Coroutine");
        animPlaying = true;
        triggerStomp();
        float timeTest = mushAnim.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(timeTest.ToString());
        // yield return new WaitUntil(()=> !mushAnim.GetCurrentAnimatorStateInfo(0).IsName("Base.MushIdle"));
        Debug.Log("Passed Not Idle");
        yield return new WaitUntil(()=> mushAnim.GetCurrentAnimatorStateInfo(0).IsName("MushIdle"));
        Debug.Log("Passed Idle");
        yield return new WaitForSeconds(timeTest);
        
        animPlaying = false;
        checkStomp();
        yield return new WaitForSeconds(cooldown);
        Debug.Log("Passed Cooldown");
        stompCooldown = false;
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
