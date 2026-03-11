using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] highestScoreText;
    [SerializeField] private TextMeshProUGUI[] highestTimeText;
    [SerializeField] private Button[] levelButton;
    [SerializeField] private string noTimeScoreText;

    private void Start()
    {
        LoadHighScores();
        LoadButtonLevel();
    }

    float highScore;
    int starScore;
    private void LoadHighScores()
    {
        for (int i = 1; i <= highestScoreText.Length; i++)
        {
            if (PlayerPrefs.HasKey("BestTime_" + i))
            {
                highScore = PlayerPrefs.GetFloat("BestTime_" + i);
                highestTimeText[i-1].text = FormatTime(highScore);
            }
            else
            {
                highestTimeText[i-1].text = noTimeScoreText;
            }

            if (PlayerPrefs.HasKey("BestScore_" + i))
            {
                starScore = PlayerPrefs.GetInt("BestScore_" + i);
                highestScoreText[i-1].text = string.Format("{0}/10", starScore);
            }
            else
            {
                highestScoreText[i-1].text = "0/10";
            }
        }
    }

    int minutes, seconds, milliSeconds;
    string FormatTime(float timeToFormat)
    {
        // คำนวณนาที วินาที และมิลลิวินาที
        minutes = Mathf.FloorToInt(timeToFormat / 60);
        seconds = Mathf.FloorToInt(timeToFormat % 60);
        milliSeconds = Mathf.FloorToInt((timeToFormat * 100) % 100);

        // 3. จัดรูปแบบ String (00:00:00)
        // :D2 คือการบังคับให้มีเลข 2 หลักเสมอ
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

    int currentLvl;
    private void LoadButtonLevel()
    {
        if (PlayerPrefs.HasKey("CurrentStage"))
        {
            currentLvl = PlayerPrefs.GetInt("CurrentStage");
        } else
        {
            currentLvl = 1;
        }

        for (int i = 0; i < levelButton.Length; i++)
        {
            if (i + 1 > currentLvl)
            {
                //Debug.Log((i + 1) + " is lessthan" + currentLvl);
                levelButton[i].interactable = false;
            }
        }
    }
}
