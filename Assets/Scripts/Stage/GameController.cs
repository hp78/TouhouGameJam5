﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{
    //
    public PlayerController player;
    public GameObject goal;

    public CinemachineVirtualCamera cinemVCam;
    public CinemachineVirtualCamera cinemVCamCinematic;

    //
    public StringSet stageNames;
    public IntVal currStageIndex;

    //
    public static GameController instance; 

    //
    public BoolVal isGamePaused;
    public BoolVal isPlayerAlive;
    public BoolVal isPlayerInControl;

    //
    public Image blackout;

    //
    public GameObject pausePanel;

    [Space(8)]
    public Image[] heartsImage;
    public Image[] bossImage;
    public Image[] succspaceImage;

    // Start is called before the first frame update
    void Start()
    {
        //
        if (instance != null)
            Destroy(instance);

        instance = this;

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        goal = GameObject.Find("Goal");

        cinemVCamCinematic.Follow = goal.transform;

        StartCoroutine(FadeIntoScene());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void OnEnable()
    {
        SuccModule.evntUpdateSuccCount += UpdateSuccCount;
        PlayerController.evntUpdateHealth += UpdateHealth;
    }

    void OnDisable()
    {
        SuccModule.evntUpdateSuccCount -= UpdateSuccCount;
        PlayerController.evntUpdateHealth -= UpdateHealth;
    }

    //
    void StageStart()
    {
        isPlayerAlive.val = true; ;
        isPlayerInControl.val = true; ;
    }

    //
    public void StageEnd()
    {
        isPlayerInControl.val = false;
        StartCoroutine(LoadIntoNextScene());
    }

    IEnumerator FadeIntoScene()
    {
        float currAlpha = 1.0f;

        cinemVCamCinematic.gameObject.SetActive(true);

        Time.timeScale = 0.0f;

        while (currAlpha > 0.0f)
        {
            currAlpha -= Time.unscaledDeltaTime;
            blackout.color = new Color(0, 0, 0, currAlpha);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        cinemVCamCinematic.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(1.1f);

        Time.timeScale = 1.0f;

        StageStart();
    }

    IEnumerator ReloadScene()
    {
        float currAlpha = 0.0f;
        Time.timeScale = 0.0f;

        while (currAlpha < 1.0f)
        {
            currAlpha += Time.unscaledDeltaTime;
            blackout.color = new Color(0, 0, 0, currAlpha);
            yield return null;
        }
        RetryScene();
    }

    IEnumerator LoadIntoNextScene()
    {
        float currAlpha = 0.0f;
        Time.timeScale = 0.0f;

        while(currAlpha < 1.0f)
        {
            currAlpha += Time.unscaledDeltaTime;
            blackout.color = new Color(0, 0, 0, currAlpha);
            yield return null;
        }
        LoadNextScene();
    }


    //
    public void RetryScene()
    {
        isGamePaused.val = false;
        SceneManager.LoadScene(stageNames.values[currStageIndex.val]);
    }

    void TogglePause()
    {
        if(isGamePaused.val)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
        isGamePaused.val = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
        isGamePaused.val = false;
    }

    //
    public void LoadNextScene()
    {
        string nextStageName = "MainMenu";
        ++currStageIndex.val;
        if ((currStageIndex.val) < stageNames.values.Length)
        {
            nextStageName = stageNames.values[currStageIndex.val];
            if(nextStageName == "Stage2-1" || nextStageName == "Stage3-1")
                JSAM.AudioManager.instance.PlayMusic("Keine");

            if (nextStageName == "Stage1-Boss") 
             JSAM.AudioManager.instance.PlayMusic("China");
            if (nextStageName == "Stage2-Boss")
                JSAM.AudioManager.instance.PlayMusic("Miko");
            if (nextStageName == "Stage3-Boss")
                JSAM.AudioManager.instance.PlayMusic("Mokou");



        }

        SceneManager.LoadScene(nextStageName);
    }

    public void ReturnToMainMenu()
    {
        isGamePaused.val = false;
        SceneManager.LoadScene("MainMenu");
    }


    void UpdateHealth(int nHealth)
    {
        if (nHealth < 1)
        {
            StartCoroutine(ReloadScene());

        }
        else
        {
            for (int i = 0; i < nHealth; ++i)
            {
                heartsImage[i].color = Color.white;
            }

            for (int i = nHealth; i < heartsImage.Length; ++i)
            {
                heartsImage[i].color = Color.black;
            }
        }
    }

    public void UpdateBossHealth(int nHealth)
    {
        if (nHealth < 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < nHealth; ++i)
            {
                bossImage[i].color = Color.blue;
            }

            for (int i = nHealth; i < bossImage.Length; ++i)
            {
                bossImage[i].color = Color.black;
            }
        }
    }

    void UpdateSuccCount(int nSuccCount, bool isSucc)
    {
        if (isSucc)
            nSuccCount += 1;

        for (int i = 0; i < nSuccCount; ++i)
        {
            succspaceImage[i].color = Color.white;
        }

        for (int i = nSuccCount; i < succspaceImage.Length; ++i)
        {
            succspaceImage[i].color = Color.black;
        }
    }
}
