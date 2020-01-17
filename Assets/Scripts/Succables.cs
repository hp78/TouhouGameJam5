using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succables : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        SuccModule.evntSuccColumn += OnSucc;
        SuccModule.evntUnsuccColumn += OnUnsucc;
    }

    private void OnDisable()
    {
        SuccModule.evntSuccColumn -= OnSucc;
        SuccModule.evntUnsuccColumn -= OnUnsucc;
    }

    //
    void OnSucc(float xPos, int columnIndex, bool isPlayerFacingLeft, SuccModule succModule)
    {
        float xDist = transform.position.x - xPos;

        if (isPlayerFacingLeft && transform.position.x < xPos)
        {
            if (xDist > -1.0f)
            {
                succModule.DepositSucc(gameObject, columnIndex);
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = transform.position + new Vector3(1.0f, 0);
            }
        }
        else if (transform.position.x > xPos)
        {
            if (xDist < 1.0f)
            {
                succModule.DepositSucc(gameObject, columnIndex);
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = transform.position - new Vector3(1.0f, 0);
            }
        }
    }

    //
    void OnUnsucc(float xPos, int columnIndex, bool isPlayerFacingLeft, SuccModule succModule)
    {
        float xDist = transform.position.x - xPos;

        if (isPlayerFacingLeft && transform.position.x < xPos)
        {
            transform.position = transform.position - new Vector3(1.0f, 0);
        }
        else if (transform.position.x > xPos)
        {
            transform.position = transform.position + new Vector3(1.0f, 0);
        }
    }
}
