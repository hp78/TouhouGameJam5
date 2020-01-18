using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space(5)]
    public BoolVal isPlayerSuccing;
    public BoolVal isGamePaused;
    public BoolVal isPlayerFacingLeft;

    [Space(5)]
    Animator anim;
    public GameObject succEffects;

    [Space(5)]
    public float moveForce;
    public float jumpForce;

    [Space(5)]
    public float jumpThreshold;

    public bool inAir;
    bool isSuccing = false;
    private bool isAttacking;

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
    public Sprite still;
    public Sprite succ;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        layermask = (1 << 8); //Player
        layermask |= (1 << 9);//enemy
        layermask |= (1 << 10);//camera
        layermask = ~layermask;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSuccing)
        {
            SuckEffect();
        }
        else
        {
            Movement();
            Jump();
        }
    }

    void Movement()
    {
        if(isPlayerSuccing.val == true)
        {
            isSuccing = true;
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
}
