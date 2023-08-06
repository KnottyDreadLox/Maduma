using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static float timeLeft;
    TextMeshProUGUI clockText;
    public static bool startClock = false;
    public static int restartCounter = 0;

    public static GameObject completedTimer;
    public static GameObject completedRestarts;

    private void Start()
    {
        clockText = GameObject.Find("TimeTxt").GetComponent<TextMeshProUGUI>();
        startClock = false;
    }

    void Update()
    {
        if (startClock == true)
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

    public static void CompleteLevel()
    {
        startClock = false;

        UIManager.gamePanel.SetActive(false);
        UIManager.endPanel.SetActive(true);

        UIManager.endTimerTxt.GetComponent<TextMeshProUGUI>().text = "Completed in : " + timeLeft.ToString("F2") + " seconds";


        if (restartCounter == 1)
        {
            UIManager.endRestartCounter.GetComponent<TextMeshProUGUI>().text = "You restarted " + restartCounter + " time !";
        }
        else
        {
            UIManager.endRestartCounter.GetComponent<TextMeshProUGUI>().text = "You restarted " + restartCounter + " times !";
        }

    }

    public static void TurnCameraClockwise()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
    }

    public static void TurnCameraAntiClockwise()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    public static void TurnCameraDefault()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }



}
