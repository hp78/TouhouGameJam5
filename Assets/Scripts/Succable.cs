using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succable : MonoBehaviour
{
    public bool isEdiable = true;
    Vector2 yzPos;

    void Start()
    {
        yzPos = new Vector2();
    }

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
            if (xDist > -1.49f && isEdiable)
            {
                SetYZ();
                succModule.DepositSucc(this, columnIndex);
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = transform.position + new Vector3(1.0f, 0);
            }
        }
        else if (!isPlayerFacingLeft && transform.position.x > xPos)
        {
            if (xDist < 1.49f && isEdiable)
            {
                SetYZ();
                succModule.DepositSucc(this, columnIndex);
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
        else if (!isPlayerFacingLeft && transform.position.x > xPos)
        {
            transform.position = transform.position + new Vector3(1.0f, 0);
        }
    }

    public void SetYZ()
    {
        yzPos = new Vector2(transform.position.y, transform.position.z);
    }

    public void SetYZ(float y, float z)
    {
        yzPos = new Vector2(y, z);
    }

    public float GetYPos()
    {
        return yzPos.x;
    }

    public float GetZPos()
    {
        return yzPos.y;
    }

    public void ResumeYZPos()
    {
        transform.position = new Vector3(transform.position.x, yzPos.x, yzPos.y);
    }
}
