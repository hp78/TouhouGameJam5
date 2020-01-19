using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHazard : MonoBehaviour
{
    public GameObject pfFireballProjectile;
    public float shootInterval = 1.0f;
    float currInterval = 0.0f;
    public float speed = 5.0f;
    //public Vector3 directionVelocity = new Vector3();
    public Transform spawnPos;

    private void Start()
    {
        Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        currInterval += Time.deltaTime;

        if(currInterval > shootInterval)
        {
            Shoot();
            currInterval = 0.0f;
        }
    }

    void Shoot()
    {
        GameObject fireball = Instantiate(pfFireballProjectile, spawnPos.position, transform.rotation);
        fireball.GetComponent<FireballProjectile>().SetDirection(transform.up * speed);
    }
}
