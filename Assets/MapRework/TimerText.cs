using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        int passedSeconds = (int)Time.realtimeSinceStartup;
        int seconds = passedSeconds % 60;
        int minutes = (passedSeconds - passedSeconds % 60) / 60;

        string minutesString = FormatTimeString(minutes);
        string secondsString = FormatTimeString(seconds);

        string output = minutesString + ":" + secondsString;

        text.text = output;
    }

    private string FormatTimeString(int inputTime) {

        string outString;


        if (inputTime < 1)
        {
            outString = "00";
        }
        else if (inputTime < 10)
        {
            outString = "0" + inputTime;
        }
        else outString = inputTime.ToString();

        return outString;
    }
}
