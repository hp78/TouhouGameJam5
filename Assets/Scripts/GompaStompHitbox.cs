using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GompaStompHitbox : MonoBehaviour
{

    public Transform obj;
    LandAIBehaviour landAI;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        landAI = obj.GetComponent<LandAIBehaviour>();
        anim = obj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag  == "Player")
        {

            if (collision.transform.position.y - 1.2f > obj.transform.position.y)
            {

                var playerrg = collision.transform.GetComponent<Rigidbody2D>();
                playerrg.velocity = new Vector2(playerrg.velocity.x, 5f);

                landAI.enabled = false;
                obj.GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                anim.Play("Death");
                Destroy(obj.gameObject, 3f);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().DamagePlayer();
            }
        }
    }
}
