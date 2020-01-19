using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandAIBehaviour : MonoBehaviour {

    public bool active;
    public float walkSpeed;
    float currSpeed;

    public Rigidbody2D rigidbody2d;
    public Vector3 movement;

    public float wallKissThreshold;

    private int layermask;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {

        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //active = false;


        layermask = (1 << 8); //Player
        layermask |= (1 << 9);//enemy
        layermask |= (1 << 10);//camera
        layermask = ~layermask;

        currSpeed = walkSpeed;
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
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, wallKissThreshold);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, wallKissThreshold);


        //if (hit || hit2)
        //{
        //    Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), hit.point, Color.cyan);
        //    Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), hit2.point, Color.cyan);
        //}

        if (hit)
        {
            spriteRenderer.flipX = true;
            currSpeed = -walkSpeed;
            
        }
        if (hit2)
        {
            spriteRenderer.flipX = false;
            currSpeed = walkSpeed;
        }
        
        movement = rigidbody2d.velocity;
        movement.x = currSpeed;

        // Set player new position
        rigidbody2d.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.transform.tag == "MainCamera")
            active = true;
    }

}
