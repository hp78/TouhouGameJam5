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

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
       
    }

    void Movement()
    {
        movement = rigidbody2d.velocity;
        movement.x = Input.GetAxis("Horizontal") * moveForce;
        rigidbody2d.velocity = movement;
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.down, jumpThreshold);//, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), Vector2.down, jumpThreshold);//, layermask);
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
}
