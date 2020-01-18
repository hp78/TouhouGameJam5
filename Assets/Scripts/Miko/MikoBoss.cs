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

    public Animator cam;
    int prevNumber;


    float idleTime;
    float spinTime;

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
        float randbool = Random.Range(0.0f, 1f);
        if (randbool <= 0.5f)
            currState = MikoState.TABLE;
        else
            currState = MikoState.SPIN;

        yield return 0;
    }

    IEnumerator Spin()
    {
        animator.Play("MikoSpin");
        for(spinCount = 0; spinCount <5; spinCount++)
        {
            float ranY = Random.Range(-2f, 2f);
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

        spriteRenderer.flipX = !rightside;
        animator.CrossFade("MikoIdle", 0.1f);
        yield return new WaitForSeconds(2f);


        if (prevState == MikoState.ROTATECAM)
            currState = MikoState.TABLE;
        else
        {
            float randbool = Random.Range(0.0f, 1f);
            if (randbool <= 0.2f)
                currState = MikoState.TABLE;
            else
                currState = MikoState.ROTATECAM;
        }
    }

    IEnumerator Throw()
    {
        int i;
        for (i = 0; i < 20; ++i)
        {

            float ranX = Random.Range(1f, 12f);
            if (rightside)
                ranX = -ranX;
            float ranY = Random.Range(10f, 15f);
            Vector2 movement = new Vector3(ranX, ranY, 0f);
            var temp = Instantiate(tablePrefab, transform.position, Quaternion.identity);
            temp.GetComponent<Rigidbody2D>().velocity = movement;
            

            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(2f);
        currState = MikoState.SPIN;
        yield return 0;

    }
    void Danmaku()
    {
        if(bulletCD < 0.0f)
        {
            float i;
            for (i = 0.0f; i > -360f; i -= 36f)
            {
                Instantiate(RanBullet, transform.position, Quaternion.Euler(0f, 0f, i));
            }
            bulletCD = 0.5f;
        }
        bulletCD -= Time.deltaTime;
    }
}
