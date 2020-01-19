using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBullet : MonoBehaviour
{

    bool sucked = false;
    int init = 1;

    public PlayerController player;
    bool face;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (init <= 0)
        {
            sucked = true;
            
            face = player.isPlayerFacingLeft.val;
            gameObject.tag = "PlayerBullet";
            transform.position = player.transform.position;
            float ranX = Random.Range(1f, 12f);
            if (face)
                ranX = -ranX;
            float ranY = Random.Range(10f, 15f);
            Vector2 movement = new Vector3(ranX, 0f, 0f);
            this.GetComponent<Rigidbody2D>().velocity = movement;

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
