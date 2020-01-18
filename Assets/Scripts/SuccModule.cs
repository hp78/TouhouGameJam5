using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccModule : MonoBehaviour
{
    #region Events

    public delegate void DeleSuccColumn(float xPos, int columnIndex, bool playerFacing, SuccModule source);
    public static event DeleSuccColumn evntSuccColumn;

    public delegate void DeleUnsuccColumn(float xPos, int columnIndex, bool playerFacing, SuccModule source);
    public static event DeleUnsuccColumn evntUnsuccColumn;

    #endregion

    public BoolVal isPlayerSuccing;
    public BoolVal isGamePaused;
    public BoolVal isPlayerFacingLeft;

    public float succOffset = 1.5f;
    public float succDelay = 0.2f;
    public float currSucc = 0.0f;

    bool isSucc = false;
    bool isUnsucc = false;
    int succCapacity = 10;
    public int currSuccCapacity = 0;

    List<List<Succable>> succList;

    // Start is called before the first frame update
    void Start()
    {
        succList = new List<List<Succable>>();
        for(int i = 0; i < 10; ++i)
        {
            succList.Add(new List<Succable>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (isGamePaused.val) return;

        //
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleSucc(true);
        }

        //
        if (Input.GetKeyUp(KeyCode.X))
        {
            ToggleSucc(false);
        }

        //
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleUnsucc(true);
        }

        //
        if (Input.GetKeyUp(KeyCode.C))
        {
            ToggleUnsucc(false);
        }

        //
        if (isSucc && CanSucc())
        {
            DoSucc(Time.deltaTime);
        }
        else if(isUnsucc && CanUnsucc())
        {
            DoUnsucc(Time.deltaTime);
        }
    }

    //
    void ToggleSucc(bool isStartSucc)
    {
        isSucc = isStartSucc;
        isPlayerSuccing.val = isSucc;
        currSucc = 0.0f;
    }

    //
    void ToggleUnsucc(bool isStartSucc)
    {
        isUnsucc = isStartSucc;
        isPlayerSuccing.val = isUnsucc;
        currSucc = 0.0f;
    }

    //
    void DoSucc(float deltaTime)
    {
        currSucc += deltaTime;

        if(currSucc > succDelay)
        {
            SuccColumn();
            currSucc = 0;
        }
    }

    //
    void DoUnsucc(float deltaTime)
    {
        currSucc += deltaTime;

        if (currSucc > succDelay)
        {
            UnsuccColumn();
            currSucc = 0;
        }
    }

    //
    void SuccColumn()
    {
        float offset = succOffset;
        if(isPlayerFacingLeft.val)
        {
            offset *= -1;
        }

        evntSuccColumn?.Invoke(transform.position.x + offset, currSuccCapacity, isPlayerFacingLeft.val,this);
        ++currSuccCapacity;
    }

    //
    void UnsuccColumn()
    {
        float offset = succOffset;
        if (isPlayerFacingLeft.val)
        {
            offset *= -1;
        }

        --currSuccCapacity;
        evntUnsuccColumn?.Invoke(transform.position.x + offset, currSuccCapacity, isPlayerFacingLeft.val,this);
        WithdrawSucc();
    }

    //
    public void DepositSucc(Succable succ, int index)
    {
        succList[index].Add(succ);
    }

    //
    public void WithdrawSucc()
    {
        //
        float offset = succOffset + 1.0f;
        if (isPlayerFacingLeft.val)
        {
            offset *= -1;
        }

        //
        foreach (Succable succ in succList[currSuccCapacity])
        {
            succ.gameObject.SetActive(true);
            float xPos = SnapTo(transform.position.x, 1.0f) + offset;
            succ.transform.position = new Vector3(xPos, succ.GetYPos(), succ.GetZPos());
        }

        succList[currSuccCapacity].Clear();
    }

    //
    bool CanSucc()
    {
        return (currSuccCapacity < succCapacity);
    }

    bool CanUnsucc()
    {
        return (currSuccCapacity > 0);
    }

    //
    public static float SnapTo(float a, float snap)
    {
        return Mathf.Round(a / snap) * snap;
    }
}
