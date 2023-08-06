using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //--------------- Variables --------------------

    private float timeLeft;
    TextMeshProUGUI clockText;

    private bool startClock = false;

    private int restartCounter = 0;

    private GameObject completedTimer;
    private GameObject completedRestarts;

    [SerializeField]
    private UIManager uiManager;

    //--------------- Encapsulation --------------------

    public bool StartClock { get => startClock; set => startClock = value; }
    public int RestartCounter { get => restartCounter; set => restartCounter = value; }

    //--------------- Methods --------------------


    private void Start()
    {
        clockText = GameObject.Find("TimeTxt").GetComponent<TextMeshProUGUI>();
        StartClock = false;
    }

    void Update()
    {
        if (StartClock == true)
        {
            timeLeft += Time.deltaTime;
            clockText.text = "Time Elapsed: " + timeLeft.ToString("F2");
        }

        //if (Input.GetKey(KeyCode.F2))
        //{
        //    TurnCameraClockwise();
        //}

        //if (Input.GetKey(KeyCode.F1))
        //{
        //    TurnCameraAntiClockwise();
        //}

        //if (Input.GetKey(KeyCode.F3))
        //{
        //    TurnCameraDefault();
        //}
    }

    public void CompleteLevel()
    {
        StartClock = false;

        uiManager.GamePanel.SetActive(false);
        uiManager.EndPanel.SetActive(true);

        uiManager.EndTimerTxt.GetComponent<TextMeshProUGUI>().text = "Completed in : " + timeLeft.ToString("F2") + " seconds";


        if (RestartCounter == 1)
        {
            uiManager.EndRestartCounter.GetComponent<TextMeshProUGUI>().text = "You restarted " + RestartCounter + " time !";
        }
        else
        {
            uiManager.EndRestartCounter.GetComponent<TextMeshProUGUI>().text = "You restarted " + RestartCounter + " times !";
        }

    }

    public void TurnCameraClockwise()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
    }

    public void TurnCameraAntiClockwise()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    public void TurnCameraDefault()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }



}
