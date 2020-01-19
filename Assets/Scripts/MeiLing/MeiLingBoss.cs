using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeiLingBoss : MonoBehaviour
{


    enum MeiState
    {
        IDLE,
        DMG,
        DROP,
        LASER,
        BOUNCE,
        DEAD,
    }

    MeiState currState;
    MeiState prevState;

    public bool active;
    public float walkSpeed;
    public float jumpForce;

    public Rigidbody2D rigidbody2d;
    public Animator anim;
    public Vector3 movement;


    public float wallKissThreshold;
    public float jumpThreshold;
    private int layermask;
    public SpriteRenderer spriteRenderer;
    public GameObject tails;

    public Transform laserPrefab;
    public Transform bulletPrefab;

    public ParticleSystem laserChargeParticle;
    public ParticleSystem laserFire;
    public ParticleSystem channel;

    public Transform goal;

    public Sprite idle;
    public Sprite charging;
    public Sprite spin;

    float bounceTime;
    float idleTime;
    bool dead=false;

    float invulFrameMax = 0.5f;
    float currInvulframe = 0.0f;
    bool isInvul = false;
   public  int bosslife = 10;

    GameController gc;

    // Start is called before the first frame update
    void Start()
    {

        rigidbody2d = this.GetComponent<Rigidbody2D>();
       // JSAM.AudioManager.instance.PlayMusic("China");
        currState = MeiState.IDLE;
        anim.Play("MeilingIdle");
        spriteRenderer.sprite = charging;

        gc = GameObject.Find("GameController").GetComponent<GameController>();
        gc.UpdateBossHealth(bosslife);
        rigidbody2d.gravityScale = 0f;
        idleTime = 3f;
        prevState = MeiState.LASER;
        bounceTime = 5f;
       // active = false;
        layermask = (1 << 8); //Player
        layermask |= (1 << 9);//enemy
        layermask |= (1 << 10);//camera
        layermask = ~layermask;
    }

    // Update is called once per frame
    void Update()
    {

        switch (currState)
        {
            case MeiState.IDLE:
                Idle();
                break;
            case MeiState.DMG:
                break;
            case MeiState.DROP:
                StartCoroutine(MikoBullets());
                break;
            case MeiState.LASER:
             StartCoroutine(MikoLazers());
                break;
            case MeiState.BOUNCE:
                BouncePhase();
                break;
            case MeiState.DEAD:
                if (!dead)
                    StartCoroutine(PlayDead());
                break;
        }

    }


    void BouncePhase()
    {

        Movement();
        Hopping();
        if (bounceTime < 0.0f)
        {
            if (currState != MeiState.DEAD)
            {
                if (prevState == MeiState.LASER)
                {
                    prevState = currState = MeiState.DROP;
                }
                else
                {
                    prevState = currState = MeiState.LASER;
                }
                anim.CrossFade("MeilingIdle", .3f);

                rigidbody2d.velocity = new Vector2(0f, 0f);
                rigidbody2d.gravityScale = 0f;
            }
        }

        bounceTime -= Time.deltaTime;
    }

    void Idle()
    {
        idleTime -= Time.deltaTime;
        if(idleTime<0.0f)
        {
            spriteRenderer.sprite = spin;
            tails.SetActive(false);
            anim.Play("MeilingSpin");
            if (currState != MeiState.DEAD)
            {
                currState = MeiState.BOUNCE;
                rigidbody2d.gravityScale = 2.5f;
                bounceTime = 5f;
            }

        }
    }



    void Movement()
    {

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, wallKissThreshold, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, wallKissThreshold, layermask);

        if (hit || hit2)
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), hit.point, Color.cyan);
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), hit2.point, Color.cyan);
        }
        if (hit)
            spriteRenderer.flipX = true;
        if (hit2)
            spriteRenderer.flipX = false;



        movement = rigidbody2d.velocity;

        if (spriteRenderer.flipX)
            movement.x = -walkSpeed;
        else
            movement.x = walkSpeed;
        // Set player new position
        rigidbody2d.velocity = movement;
    }

    void Hopping()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, jumpThreshold, layermask);
       
        if(hit)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
        }
    }

    IEnumerator MikoLazers()
    {

        spriteRenderer.sprite = idle;
        tails.SetActive(true);
        currState = MeiState.IDLE;
        idleTime = 6f;
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.sprite = charging;
        laserChargeParticle.Play();

        while (transform.position.y < 6.5f)
        {
            transform.position= Vector2.MoveTowards(transform.position, transform.position + new Vector3(0f, 6.5f, 0f), 1f*Time.deltaTime);
            yield return null;

        }

        float i;
        for(i = 0.0f; i>-360f; i-=36f)
        {
            Instantiate(laserPrefab, transform.position, Quaternion.Euler(0f, 0f, i));
            Instantiate(laserPrefab, transform.position, Quaternion.Euler(0f, 0f, -i+180f));

            yield return new WaitForSeconds(0.03f);
        }
        laserFire.Play();

        yield return 0;
    }

    IEnumerator MikoBullets()
    {
        spriteRenderer.sprite = idle;
        tails.SetActive(true);

        currState = MeiState.IDLE;
        idleTime = 6f;
        yield return new WaitForSeconds(.5f);
        spriteRenderer.sprite = charging;
        channel.Play();
        while (transform.position.y < 5.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(0f, 5.5f, 0f), 1f * Time.deltaTime);
            yield return null;

        }

        int i;
        for (i = 0; i < 20; ++i)
        {

            float ranX = Random.Range(-6f, 6f);
            float ranY = Random.Range(4f, 6f);
            Vector2 newpos = transform.position + new Vector3(ranX, ranY, 0f);
            Instantiate(bulletPrefab, newpos, Quaternion.identity);

            yield return new WaitForSeconds(0.25f);
        }
        yield return 0;

    }


    public IEnumerator ReceiveDamage()
    {
        currInvulframe = 0.0f;
        isInvul = true;

        --bosslife;
        gc.UpdateBossHealth(bosslife);

        if (bosslife <= 0)
            currState = MeiState.DEAD;

        while (currInvulframe < invulFrameMax)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            currInvulframe += 0.1f;

            spriteRenderer.color = Color.black;

            yield return new WaitForSeconds(0.05f);

            currInvulframe += 0.05f;
        }

        spriteRenderer.color = Color.white;

        isInvul = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);
            if (!isInvul)
            {
                StartCoroutine(ReceiveDamage());
            }


        }
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.y - 1.2f < transform.position.y)
                collision.GetComponent<PlayerController>().DamagePlayer();
        }
    }

    IEnumerator PlayDead()
    {

        rigidbody2d.velocity = new Vector2(0f, 0f);
        rigidbody2d.gravityScale = 0f;
        dead = true;
        GetComponent<Collider2D>().enabled = false;
        tails.SetActive(true);
        spriteRenderer.sprite = charging;
        anim.Play("MeilingDead");
        while (goal.position.y > 2f)
        {
            goal.position = Vector2.MoveTowards(goal.position, goal.position + new Vector3(0f, -1f, 0f), 1f * Time.deltaTime);
            yield return null;

        }

    }



}
