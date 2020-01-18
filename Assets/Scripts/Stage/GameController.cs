using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //
    public string nextSceneName;

    //
    public static GameController instance; 

    //
    public BoolVal isGamePaused;
    public BoolVal isPlayerAlive;
    public BoolVal isPlayerInControl;

    //
    public Image blackout;

    [Space(8)]
    public Image[] heartsImage;
    public Image[] succspaceImage;

    // Start is called before the first frame update
    void Start()
    {
        //
        if (instance != null)
            Destroy(instance);

        instance = this;
        StartCoroutine(FadeIntoScene());
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
    public void StageEnd()
    {
        isPlayerInControl.val = false;
        StartCoroutine(LoadIntoNextScene());
    }

    IEnumerator FadeIntoScene()
    {
        float currAlpha = 1.0f;
        Time.timeScale = 1.0f;

        while (currAlpha > 0.0f)
        {
            currAlpha -= Time.deltaTime;
            blackout.color = new Color(0, 0, 0, currAlpha);
            yield return null;
        }
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
    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }


    void UpdateHealth(int nHealth)
    {
        for(int i = 0; i < nHealth; ++i)
        {
            heartsImage[i].color = Color.white;
        }

        for (int i = nHealth; i < heartsImage.Length; ++i)
        {
            heartsImage[i].color = Color.black;
        }
    }

    void UpdateSuccCount(int nSuccCount)
    {
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
