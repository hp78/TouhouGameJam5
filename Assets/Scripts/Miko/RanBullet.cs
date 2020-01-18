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
}
