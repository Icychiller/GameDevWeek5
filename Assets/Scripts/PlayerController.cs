using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MarioAudioTag{
    marioJump=0,
    marioWalk=1,
    muda = 2,
    fireball = 3
}

[System.Serializable]
public class MarioAudioClip
{
    public AudioClip audioClip;
    public MarioAudioTag audioTag;
}

public class PlayerController : MonoBehaviour
{
    // Class Attributes
    [Header("States")]
    public bool freezeState = false;
    public bool onGroundState = true;
    public bool currentFaceRight = true;
    public bool godMode = false;

    // Import Variables
    
    [Header("Scriptable Objects")]
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public FloatVariable currentX;
    public FloatVariable currentY;
    public BoolVariable godState;
    public BoolVariable marioFaceRight;
    public GameConstants gameConstants;
    public CustomPlayerCastEvent OnPlayerCast;
    
    private float force;

    [Header("Game Objects")]
    public Sprite deadSprite;
    public ParticleSystem somethingWeird;
    public GameObject leftHitBox;
    public GameObject rightHitBox;

    // Component Imports
    private AudioSource marioAudio;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Animator marioAnimator;

    // Audio Clips
    public List<MarioAudioClip> marioAudioList;

    // Ability Variables
    [Header("Abilities")]
    public BoolVariable fireball;
    public BoolVariable muda;
    public BoolVariable mudaRunState;
    public BoolVariable playerOnGround;
    public GameObject projectile;
    public GameObject mudaEffectParent;
    private int mudaCounter = 0;
    private int maxMuda;
    private bool intervalCooldown = false;
    [SerializeField]
    private bool activeFireball = false;
    [SerializeField]
    private bool activeMuda = false;

    // Start is called before the first frame update
    void Start()
    {
        marioAudio = GetComponent<AudioSource>();
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        maxMuda = mudaEffectParent.transform.childCount;
        mudaRunState.SetValue(false);
        playerOnGround.SetValue(onGroundState);

        marioMaxSpeed.SetValue(gameConstants.playerStartingMaxSpeed);
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        updatePlayerValues();
        force = gameConstants.playerDefaultForce;
    }

    void updatePlayerValues()
    {
        currentX.SetValue(this.transform.position.x);
        currentY.SetValue(this.transform.position.y);
        godState.SetValue(this.godMode);
        marioFaceRight.SetValue(this.currentFaceRight);
    }

    void FixedUpdate()
    {
        if (!freezeState)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            // Accelerate Movement
            if (Mathf.Abs(moveHorizontal)>0){
                Vector2 movement = new Vector2(moveHorizontal*force, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.value){
                    marioBody.AddForce(movement);
                }
            }
            
            // Stop movement
            if (!Input.GetKey("a") && !Input.GetKey("d")){
                marioBody.velocity = new Vector2(0, marioBody.velocity.y);
            }

            // Extra: Jump Higher while spacebar is held
            if (Input.GetKey("space")){
                marioBody.gravityScale = gameConstants.gravityLow;
                if (onGroundState)
                {
                    marioBody.AddForce(Vector2.up * marioUpSpeed.value, ForceMode2D.Impulse);
                    onGroundState = false;
                    playerOnGround.SetValue(false);
                }
            }
            else {
                marioBody.gravityScale = gameConstants.gravityNormal;
            }
        }
    }

    void flipMario()
    {
        currentFaceRight = !marioSprite.flipX;
        Vector3 mudaRot = mudaEffectParent.transform.rotation.eulerAngles;
        mudaRot = new Vector3(mudaRot.x,mudaRot.y + 180, mudaRot.z);
        mudaEffectParent.transform.rotation = Quaternion.Euler(mudaRot);
        
        rightHitBox.SetActive(currentFaceRight);
        leftHitBox.SetActive(!currentFaceRight);
    }

    // Update is called once per frame
    void Update()
    {
        // Update Values read by other classes
        updatePlayerValues();

        // Mario Animator parameters
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround",onGroundState);


        if(!freezeState)
        {
            if ((Input.GetKey("a")||Input.GetKey("d")))
            {
                marioSprite.flipX = (Input.GetAxis("Horizontal")<0);
                if (Mathf.Abs(marioBody.velocity.x)>1.0 && (currentFaceRight == marioSprite.flipX))
                {
                    marioAnimator.SetTrigger("onSkid");
                    flipMario();
                }
                else if (currentFaceRight == marioSprite.flipX)
                {
                    flipMario();
                }
            }

            if (Input.GetKey("z"))
            {
                OnPlayerCast.Invoke(KeyCode.Z);
            }
            if (Input.GetKey("x"))
            {
                OnPlayerCast.Invoke(KeyCode.X);
            }
            if (Input.GetKey("c"))
            {
                OnPlayerCast.Invoke(KeyCode.C);
            }
            if (Input.GetKey("q"))
            {
                activateMuda();
            }
            if (!Input.GetKey("q") || Input.GetKeyUp("q"))
            {
                if (muda.value || activeMuda)
                {
                    deactivateMuda();
                }
            }
            if (Input.GetKeyDown("e"))
            {
                launchFireball();
            }
        }
    }

    void deactivateMuda()
    {
        if (mudaRunState.value)
        {
            mudaRunState.SetValue(false);
        }
        
        if(marioAudio.isPlaying)
        {
            marioAudio.Stop();
        }
        for (int index = 0; index < mudaEffectParent.transform.childCount; index++)
        {
            if (mudaEffectParent.transform.GetChild(index).gameObject.activeInHierarchy)
            {
                mudaEffectParent.transform.GetChild(index).gameObject.SetActive(false);
            }
            
        }
    }
    void activateMuda()
    {
        
        
        if (!intervalCooldown && (activeMuda || muda.value))
        {
            if (!mudaRunState.value)
            {
                mudaRunState.SetValue(true);
            }
            if (!marioAudio.isPlaying)
            {
                foreach (MarioAudioClip ab in marioAudioList)
                {
                    if(ab.audioTag == MarioAudioTag.muda)
                    {
                        marioAudio.PlayOneShot(ab.audioClip);
                        break;
                    }
                }
            }

            

            GameObject subMuda = mudaEffectParent.transform.GetChild(mudaCounter).gameObject;

            if (!subMuda.activeInHierarchy)
            {
                subMuda.SetActive(true);
            }

            if(mudaCounter < maxMuda)
            {
                mudaCounter++;
            }
            if(mudaCounter >= maxMuda)
            {
                mudaCounter = 0;
            }
            intervalCooldown = true;
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(gameConstants.mudaCooldown);
        intervalCooldown = false;
    }

    void launchFireball()
    {
        if (activeFireball || fireball.value)
        {
            Debug.Log("Pew pew pew");
            foreach(MarioAudioClip mac in marioAudioList)
            {
                if (mac.audioTag == MarioAudioTag.fireball)
                {
                    marioAudio.PlayOneShot(mac.audioClip);
                }
            }
            
            float xDirection = 1f;
            if (!currentFaceRight)
            {
                xDirection = -1f;
            }
            Instantiate(projectile, new Vector3(this.transform.position.x+xDirection, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach(ContactPoint2D contactPoint in col.contacts)
        {
            //Debug.Log(contactPoint.normal);
            if(contactPoint.normal.y > 0)
            {
                //Debug.Log("Mario is On something. It might be shrooms");
                onGroundState = true;
                playerOnGround.SetValue(true);
                somethingWeird.Play();
                break;
            }
        }
    }

    public void PlayerDiesSequence()
    {
        this.freezeState = true;
        this.marioBody.bodyType = RigidbodyType2D.Kinematic;
        this.marioSprite.sprite = deadSprite;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<Rotator>().enabled = true;
        StartCoroutine(floatAndSpinDeath());
    }

    IEnumerator floatAndSpinDeath()
    {
        for (int i = 0; i<300; i++)
        {
            this.marioBody.MovePosition(new Vector2(this.marioBody.position.x, this.marioBody.position.y + 0.02f));
            yield return new WaitForEndOfFrame();
        }
        
    }

    public void PlayerCompleteLevel()
    {
        this.freezeState = true;
    }

    void PlayJumpSound(){
        marioAudio.PlayOneShot(marioAudio.clip);
    }
}
