using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static GameObject gamePanel;
    public static GameObject endPanel;

    public static GameObject endTimerTxt;
    public static GameObject endRestartCounter;


    void Start()
    {
        gamePanel = GameObject.Find("GamePanel");
        gamePanel.SetActive(true);

        endTimerTxt = GameObject.Find("CompletedTimeTxt");
        endRestartCounter = GameObject.Find("RestartCounterTxt");

        endPanel = GameObject.Find("EndPanel");
        endPanel.SetActive(false);
    }

    public void RestartLevelInGame()
    {
        GameManager.restartCounter++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        GameManager.restartCounter = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadLevel(string level)
    {

        SceneManager.LoadScene("Level_" + level);
    }

    public void NextLevel()
    {
        GameManager.restartCounter = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
