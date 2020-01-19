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
        StartGame();
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
