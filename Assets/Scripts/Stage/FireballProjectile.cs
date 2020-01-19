using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector3 direction = new Vector3();

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction;
    }

    private void OnEnable()
    {
        if (!rb2d) return;
        rb2d.velocity = direction;
    }

    public void SetDirection(Vector3 directionVelo)
    {
        direction = directionVelo;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Player"))
        {
            go.GetComponent<PlayerController>().DamagePlayer();
        }

        Destroy(gameObject);
    }
}
