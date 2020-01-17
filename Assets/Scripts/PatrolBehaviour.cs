using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour {

    public float PatrolSpeed;

    private bool reverse;
    public Transform endPoint;
    private Vector3 startPoint;


	// Use this for initialization
	void Start () {

        startPoint = this.transform.position;
        

	}
	
	// Update is called once per frame
	void Update () {

        if (reverse)
            this.transform.position = Vector2.MoveTowards(this.transform.position, startPoint, Time.deltaTime * PatrolSpeed);
        else
            this.transform.position = Vector2.MoveTowards(this.transform.position, endPoint.position, Time.deltaTime * PatrolSpeed);

        if (this.transform.position == endPoint.position)
            reverse = true;
        if (this.transform.position == startPoint)
            reverse = false;

    }
}
