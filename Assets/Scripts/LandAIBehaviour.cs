using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandAIBehaviour : MonoBehaviour {

    public bool active;
    public float walkSpeed;

    public Rigidbody2D rigidbody2d;
    public Vector3 movement;

    public float wallKissThreshold;

    private int layermask;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {

        rigidbody2d = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        active = false;


        layermask = (1 << 8); //Player
        layermask |= (1 << 9);//enemy
        layermask |= (1 << 10);//camera
        layermask = ~layermask;
    }
	
	// Update is called once per frame
	void Update () {
		
        if(active)
        {
            Movement();
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
        if(hit2)
            spriteRenderer.flipX = false;


        movement = rigidbody2d.velocity;

        if(spriteRenderer.flipX)
            movement.x = -walkSpeed;
        else
            movement.x = walkSpeed;
        // Set player new position
        rigidbody2d.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.transform.tag == "MainCamera")
            active = true;
    }
}
