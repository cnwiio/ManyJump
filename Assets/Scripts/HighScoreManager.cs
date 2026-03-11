using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] highestScoreText;
    [SerializeField] private TextMeshProUGUI[] highestTimeText;
    [SerializeField] private string noTimeScoreText;

    private void Start()
    {
        LoadHighScores();
    }

    float highScore;
    int starScore;
    private void LoadHighScores()
    {
        for (int i = 0 /*change to 1 later*/; i < highestScoreText.Length /* +1 later*/; i++)
        {
            if (PlayerPrefs.HasKey("BestTime_" + i))
            {
                highScore = PlayerPrefs.GetFloat("BestTime_" + i);
                highestTimeText[i].text = FormatTime(highScore);
            }
            else
            {
                highestTimeText[i].text = noTimeScoreText;
            }

            if (PlayerPrefs.HasKey("BestScore_" + i))
            {
                starScore = PlayerPrefs.GetInt("BestScore_" + i);
                highestScoreText[i].text = string.Format("{0}/10", starScore);
            }
            else
            {
                highestScoreText[i].text = "0/10";
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
}
