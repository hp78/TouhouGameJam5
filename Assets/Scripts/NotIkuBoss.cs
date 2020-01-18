using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotIkuBoss : MonoBehaviour
{

    
    enum IkuStates
    {
        IDLE,
        DMG,
        BOLT,
        DRILL,
        DANMAKU,
        DEAD,
    }

    IkuStates currState;

    float idleTime;
    float dmgTime;

    public Transform drillpoint1;
    public Transform drillpoint2;
    float drillDelay;
    float drillSpeed;

    public Transform player;
    public SpriteRenderer srender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState)
        {
            case IkuStates.IDLE:
                break;
            case IkuStates.DMG:
                break;
            case IkuStates.BOLT:
                break;
            case IkuStates.DRILL:
                break;
            case IkuStates.DANMAKU:
                break;
            case IkuStates.DEAD:
                break;
        }
    }

    void Idle()
    {
        idleTime -= Time.deltaTime;
        if(idleTime <0.0f)
        {
            var rand = Random.Range(1,4);
            if (rand == 1)
                currState = IkuStates.BOLT;
            else if (rand == 2)
            {
                currState = IkuStates.DRILL;
                drillDelay = 2f;
            }
            else if (rand == 3)
                currState = IkuStates.DANMAKU;
        }
    }

    void Drill()
    {
        drillDelay -= Time.deltaTime;
        if(drillDelay < 0.0f)
        {
        }
    }
    
}
