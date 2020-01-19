using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikoBullet : MonoBehaviour
{
    public float speedIncrement;
    float speed;

    bool sucked = false;
    int init = 1;
    public PlayerController player;
    bool face;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sucked)
        {
            this.transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(0f, -1f, 0f), speed * Time.deltaTime);
            speed += speedIncrement * Time.deltaTime;
        }
        else
        {
            if(face)
            {
                this.transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(-1f, 0f, 0f), speed * Time.deltaTime);
                speed += speedIncrement * Time.deltaTime;
            }
            else
            {
                this.transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(1f, 0f, 0f), speed * Time.deltaTime);
                speed += speedIncrement * Time.deltaTime;
            }
        }
    }

    private void OnEnable()
    { if (init <= 0)
        {
            sucked = true;
            //transform.position = player.transform.position;
            face = player.isPlayerFacingLeft.val;
            gameObject.tag = "PlayerBullet";
        }
        --init;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BulletDestroyer")
            Destroy(gameObject);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletDestroyer")
            Destroy(gameObject);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().DamagePlayer();
        }
    }

}
