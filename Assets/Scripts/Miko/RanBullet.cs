using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanBullet : MonoBehaviour
{

    public float speedIncrement;
    public float Initspeed;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Initspeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + (transform.up*10f), Time.deltaTime * speed);
        speed += Time.deltaTime * speedIncrement;
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
