using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public IntVal currentStageIndex;
    public StringSet stageNames;

    // Start is called before the first frame update
    void Start()
    {
        JSAM.AudioManager.instance.PlayMusic("MainMenu");
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            StartGame();
            JSAM.AudioManager.instance.PlayMusic("Keine");

        }
    }

    public void StartGame()
    {
        StartGame(0);
    }

    public void StartGame(int stage)
    {
        currentStageIndex.val = stage;
        SceneManager.LoadScene(stageNames.values[stage]);
    }
}
