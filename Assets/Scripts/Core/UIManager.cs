using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //--------------- Variables --------------------

    [SerializeField] 
    private GameObject gamePanel;

    [SerializeField]
    private GameObject endPanel;

    [SerializeField]
    private GameObject endTimerTxt;

    [SerializeField]
    private GameObject endRestartCounter;

    [SerializeField]
    private GameManager gameManager;

    //--------------- Encapsulation --------------------

    public GameObject GamePanel { get => gamePanel; set => gamePanel = value; }
    public GameObject EndPanel { get => endPanel; set => endPanel = value; }
    public GameObject EndTimerTxt { get => endTimerTxt; set => endTimerTxt = value; }
    public GameObject EndRestartCounter { get => endRestartCounter; set => endRestartCounter = value; }

    //--------------- Methods --------------------

    void Start()
    {
        //GamePanel = GameObject.Find("GamePanel");
        GamePanel.SetActive(true);

        EndTimerTxt = GameObject.Find("CompletedTimeTxt");
        EndRestartCounter = GameObject.Find("RestartCounterTxt");

        //EndPanel = GameObject.Find("EndPanel");
        EndPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        gameManager.RestartCounter = 0;
        SceneManager.LoadScene(0);
    }

    public void RestartLevelInGame()
    {
        gameManager.RestartCounter++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        gameManager.RestartCounter = 0;
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
        gameManager.RestartCounter = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
