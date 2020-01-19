using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikoBoss : MonoBehaviour
{


    enum MikoState
    {
        IDLE,
        DANMAKU,
        ROTATECAM,
        TABLE,
        SPIN,
        DEAD,
    }

    MikoState currState;
    MikoState prevState;

    Animator animator;

    public Transform leftSide;
    public Transform rightSide;

    bool rightside;
    bool reached;
    int spinCount;
    float bulletCD;

    public bool active;
    public float floatSpeed;

    public SpriteRenderer spriteRenderer;

    public Transform tablePrefab;
    public Transform RanBullet;
    public Transform goal;

    public Animator cam;
    int prevNumber;
    bool dead = false;


    float invulFrameMax = 1f;
    float currInvulframe = 0.0f;
    bool isInvul = false;
    public int bosslife = 10;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        prevNumber = 1;
        currState = MikoState.SPIN;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Spin());
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(Throw());
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(RotateCamera());


        switch (currState)
        {
            case MikoState.IDLE:
                break;
            case MikoState.DANMAKU:
                Danmaku();
                break;
            case MikoState.ROTATECAM:
                StartCoroutine(RotateCamera());
                prevState = MikoState.ROTATECAM;
                currState = MikoState.IDLE;
                break;
            case MikoState.TABLE:
                StartCoroutine(Throw());
                prevState = MikoState.TABLE;
                currState = MikoState.IDLE;

                break;
            case MikoState.SPIN:
                StartCoroutine(Spin());
                currState = MikoState.DANMAKU;

                break;
            case MikoState.DEAD:
                if (!dead)
                {
                    StopAllCoroutines();
                    StartCoroutine(PlayDead());
                }
                break;
        }




    }

    IEnumerator RotateCamera()
    {
        int rand = Random.Range(1, 5);
        while(prevNumber == rand)
            rand = Random.Range(1, 5);
        switch (rand)
        {
            case 1:
                cam.CrossFade("CamNeutral", 0.2f);
                break;
            case 2:
                cam.CrossFade("CamRotate90", 0.2f);
                break;
            case 3:
                cam.CrossFade("CamRotate180", 0.2f);
                break;
            case 4:
                cam.CrossFade("CamRotate270", 0.2f);
                break;
        }
        prevNumber = rand;
        yield return new WaitForSeconds(3f);
        if (currState != MikoState.DEAD)
        {
            float randbool = Random.Range(0.0f, 1f);
            if (randbool <= 0.5f)
                currState = MikoState.TABLE;
            else
                currState = MikoState.SPIN;
        }
        yield return 0;
    }

    IEnumerator Spin()
    {
        animator.Play("MikoSpin");
        for(spinCount = 0; spinCount <5; spinCount++)
        {
            float ranY = Random.Range(-2f, 1f);
            while(!reached)
            {
                if (rightside)
                {
                    transform.position = Vector2.MoveTowards(transform.position, leftSide.position + new Vector3(0f, ranY, 0f), floatSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, leftSide.position + new Vector3(0f, ranY, 0f)) < 0.1f)
                    {
                        reached = true;
                    }
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, rightSide.position + new Vector3(0f, ranY, 0f), floatSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, rightSide.position + new Vector3(0f, ranY, 0f)) < 0.1f)
                    {
                        reached = true;
                    }
                }
                yield return null;
            }
            rightside = !rightside;
            reached = false;
        }
        currState = MikoState.IDLE;

        animator.CrossFade("MikoIdle", 0.1f);
        spriteRenderer.flipX = !rightside;

        yield return new WaitForSeconds(2f);

        if (currState != MikoState.DEAD)
        {
            if (prevState == MikoState.ROTATECAM)
                currState = MikoState.TABLE;
            else
            {
                float randbool = Random.Range(0.0f, 1f);
                if (randbool <= 0.15f)
                    currState = MikoState.TABLE;
                else
                {
                    animator.Play("MikoFlip");
                    yield return new WaitForSeconds(1f);
                    if (currState != MikoState.DEAD)
                        currState = MikoState.ROTATECAM;
                }
            }
        }
    }

    IEnumerator Throw()
    {
        int i;
        while (!reached)
        {
            if (!rightside)
            {
                transform.position = Vector2.MoveTowards(transform.position, leftSide.position + new Vector3(0f, -4.5f, 0f), floatSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, leftSide.position + new Vector3(0f, -4.5f, 0f)) < 0.1f)
                {
                    reached = true;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, rightSide.position + new Vector3(0f, -4.5f, 0f), floatSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, rightSide.position + new Vector3(0f, -4.5f, 0f)) < 0.1f)
                {
                    reached = true;
                }
            }
            yield return null;
        }
        reached = false;

        yield return new WaitForSeconds(2f);

        animator.Play("MikoTable");

        for (i = 0; i < 10; ++i)
        {

            float ranX = Random.Range(5f, 10f);
            if (rightside)
                ranX = -ranX;
            float ranY = Random.Range(5f, 15f);
            Vector2 movement = new Vector3(ranX, ranY, 0f);
            var temp = Instantiate(tablePrefab, transform.position, Quaternion.identity);
            temp.GetComponent<Rigidbody2D>().velocity = movement;
            

            yield return new WaitForSeconds(0.50f);
        }
        animator.CrossFade("MikoIdle", 0.1f);

        yield return new WaitForSeconds(3f);
        if (currState != MikoState.DEAD)
            currState = MikoState.SPIN;
        yield return 0;

    }
    void Danmaku()
    {
        if(bulletCD < 0.0f)
        {
            float i;
            for (i = 0.0f; i > -360f; i -= 120)
            {
                Instantiate(RanBullet, transform.position, Quaternion.Euler(0f, 0f, i+60));
            }
            bulletCD = 0.5f;
        }
        bulletCD -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
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

    public IEnumerator ReceiveDamage()
    {
        currInvulframe = 0.0f;
        isInvul = true;
        --bosslife;
        if (bosslife <= 0)
            currState = MikoState.DEAD;

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


    IEnumerator PlayDead()
    {


        dead = true;
        GetComponent<Collider2D>().enabled = false;
        goal.gameObject.SetActive(true);
        animator.Play("MikoDead");
        while (goal.position.y > 2f)
        {
            goal.position = Vector2.MoveTowards(goal.position, goal.position + new Vector3(0f, -1f, 0f), 1f * Time.deltaTime);
            yield return null;

        }

    }
}
