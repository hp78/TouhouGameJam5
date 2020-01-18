using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikoBullet : MonoBehaviour
{
    public float speedIncrement;
    float speed;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(0f, -1f, 0f), speed * Time.deltaTime);
        speed += speedIncrement*Time.deltaTime;
    }
}
