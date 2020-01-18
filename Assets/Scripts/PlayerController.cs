using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveForce;
    public float jumpForce;

    public float jumpThreshold;

    public bool inAir;
    private bool isAttacking;

    public Rigidbody2D rigidbody2d;
    public Vector3 movement;
    private int layermask;

    public ParticleSystem suck1effect;
    public ParticleSystem suck2effect;
    public ParticleSystem suck3effect;

    public SpriteRenderer sRender;
    public Sprite still;
    public Sprite succ;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = this.GetComponent<Rigidbody2D>();
        layermask = (1 << 8); //Player
        layermask |= (1 << 9);//enemy
        layermask |= (1 << 10);//camera
        layermask = ~layermask;

    }

    // Update is called once per frame
    void Update()
    {

        Movement();
        Jump();
        SuckEffect();

        
       
    }

    void Movement()
    {
        movement = rigidbody2d.velocity;
        movement.x = Input.GetAxis("Horizontal") * moveForce;
        rigidbody2d.velocity = movement;
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), Vector2.down, jumpThreshold, layermask);
        Gizmos.color = Color.cyan;
        if (hit || hit2)
        {
            inAir = false;


      
            Debug.DrawLine(new Vector2(transform.position.x + 0.5f, transform.position.y), hit.point, Color.cyan);
            Debug.DrawLine(new Vector2(transform.position.x - 0.5f, transform.position.y), hit2.point, Color.cyan);
        }

        else
        {
            inAir = true;
          
        }

        if (Input.GetKey(KeyCode.Z) && !inAir)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);

        }
    }

    void SuckEffect()
    {
        if (Input.GetKey(KeyCode.X))
        {
            sRender.sprite = succ;
            //suck1effect.Play();
            suck2effect.Play();
        }
        else if(Input.GetKey(KeyCode.C))
        {
            sRender.sprite = succ;
            suck3effect.Play();
        }
        else
        sRender.sprite = still;

    }
}
