using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public delegate void DeleUpdateHealth(int health);
    public static event DeleUpdateHealth evntUpdateHealth;

    [Space(5)]
    public BoolVal isPlayerSuccing;
    public BoolVal isGamePaused;
    public BoolVal isPlayerFacingLeft;
    public BoolVal isPlayerAlive;
    public BoolVal isPlayerInControl;

    [Space(5)]
    Animator anim;
    public GameObject succEffects;

    [Space(5)]
    public float moveForce;
    public float jumpForce;

    [Space(5)]
    bool isPlayerInvul = false;
    float invulFrameMax = 0.5f;
    float currInvulframe = 0.0f;

    [Space(5)]
    public float jumpThreshold;

    public bool inAir;
    bool isSuccing = false;
    bool isAttacking = false;
    int currHealth = 3;

    public Rigidbody2D rigidbody2d;
    public Vector3 movement;
    private int layermask;

    [Space(5)]
    public ParticleSystem suck1effect;
    public ParticleSystem suck2effect;
    public ParticleSystem unsuck1effect;
    public ParticleSystem unsuck2effect;

    [Space(5)]
    public GameObject spriteBody;
    public SpriteRenderer trueSprite;
    public Sprite still;
    public Sprite succ;

    Transform goal;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currHealth = 3;

        layermask = (1 << 8); //Player
        layermask |= (1 << 9);//enemy
        layermask |= (1 << 10);//camera
        layermask = ~layermask;
        goal = GameObject.Find("Goal").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerAlive.val || isGamePaused.val || !isPlayerInControl) return;

        if(isSuccing)
        {
            SuckEffect();
        }
        else
        {
            Movement();
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            this.transform.position = goal.position;
        }

    }

    void Movement()
    {
        if(isPlayerSuccing.val == true)
        {
            isSuccing = true;
            movement = rigidbody2d.velocity;
            movement.x = 0;
            rigidbody2d.velocity = movement;

            return;
        }

        movement = rigidbody2d.velocity;
        movement.x = Input.GetAxis("Horizontal") * moveForce;

        if(movement.x < 0)
        {
            isPlayerFacingLeft.val = true;
            spriteBody.transform.localScale = new Vector3(1, spriteBody.transform.localScale.y, spriteBody.transform.localScale.z);
            succEffects.transform.localScale = new Vector3(-1, succEffects.transform.localScale.y, succEffects.transform.localScale.z);
            anim.SetBool("IsFacingLeft", true);
        }
        else if(movement.x > 0)
        {
            isPlayerFacingLeft.val = false;
            spriteBody.transform.localScale = new Vector3(-1, spriteBody.transform.localScale.y, spriteBody.transform.localScale.z);
            succEffects.transform.localScale = new Vector3(1, succEffects.transform.localScale.y, succEffects.transform.localScale.z);
            anim.SetBool("IsFacingLeft", false);
        }

        anim.SetFloat("XVelocity", movement.x);
        anim.SetFloat("YVelocity", movement.y);

        rigidbody2d.velocity = movement;

        if (transform.position.y < -5.0f)
        {
            currHealth = 0;
            DamagePlayer();
        }
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        //Gizmos.color = Color.cyan;

        if (hit || hit2)
        {
            inAir = false;
            anim.SetTrigger("TriggerLand");

            //Debug.DrawLine(new Vector2(transform.position.x + 0.5f, transform.position.y), hit.point, Color.cyan);
            //Debug.DrawLine(new Vector2(transform.position.x - 0.5f, transform.position.y), hit2.point, Color.cyan);
        }
        else
        {
            inAir = true;
        }
        
        if (!inAir &&Input.GetKeyDown(KeyCode.Z))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
            anim.SetTrigger("TriggerJump");
        }


    }

    void SuckEffect()
    {
        if (!isPlayerSuccing.val)
        {
            //sRender.sprite = still;
            anim.SetBool("IsSuccing", false);
            isSuccing = false;
        }
        else if (Input.GetKey(KeyCode.X))
        {
                //sRender.sprite = succ;
                suck1effect.Play();
                suck2effect.Play();
                anim.SetBool("IsSuccing", true);
        }
        else if (Input.GetKey(KeyCode.C))
        {
                //sRender.sprite = succ;
                unsuck1effect.Play();
                unsuck2effect.Play();
                anim.SetBool("IsSuccing", true);
        }

    }

    public void DamagePlayer()
    {
        if (isPlayerInvul) return;

        --currHealth;

        if(currHealth < 1)
        {
            isPlayerAlive.val = false;
        }
        else
        {
            StartCoroutine(ReceiveDamage());
        }
        evntUpdateHealth?.Invoke(currHealth);
    }

    IEnumerator ReceiveDamage()
    {
        currInvulframe = 0.0f;
        isPlayerInvul = true;

        while (currInvulframe < invulFrameMax)
        {
            trueSprite.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            currInvulframe += 0.1f;

            trueSprite.color = Color.black;

            yield return new WaitForSeconds(0.05f);

            currInvulframe += 0.05f;
        }

        trueSprite.color = Color.white;
        isPlayerInvul = false;
    }

    public void HealPlayer()
    {
        ++currHealth;

        if (currHealth > 3)
        {
            currHealth = 3;
        }
        evntUpdateHealth?.Invoke(currHealth);
    }

}

