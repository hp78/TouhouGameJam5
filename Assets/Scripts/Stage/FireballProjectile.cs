using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector3 direction;
    float speed = 1.0f;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetDirection()
    {

    }
}
