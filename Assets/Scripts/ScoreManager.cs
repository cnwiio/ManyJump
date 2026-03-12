using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI starText;

    private int starScore;
    private float elapsedTime = 0f;
    private bool isRunning = true;

    private string currentScenePrefName, bestTimePrefName, bestScorePrefName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerScripts.Instance.OnDeath += StopCounter; // รีเซ็ตเวลาเมื่อผู้เล่นตาย
        PlayerScripts.Instance.OnCollectStar += AddStar; // เพิ่มดาวเมื่อเก็บดาวได้
        GameManager.Instance.RestartEvent += RestartCounter; // รีเซ็ตเวลาเมื่อเกมรีสตาร์ท
        GameManager.Instance.WinEvent += CompleteStage;

        currentScenePrefName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        //Debug.Log(currentScenePrefName);
        //currentScenePrefName = "0";
        bestTimePrefName = "BestTime_" + currentScenePrefName;
        bestScorePrefName = "BestScore_" + currentScenePrefName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return; // ถ้าไม่กำลังนับเวลา ให้ข้ามการอัปเดต

        // 1. สะสมเวลาที่ผ่านไปในแต่ละเฟรม
        elapsedTime += Time.deltaTime;

        UpdateTimerDisplay(elapsedTime);
        UpdateStarDisplay(starScore);
    }

    int minutes, seconds, milliSeconds;
    void UpdateTimerDisplay(float timeToDisplay)
    {
        // คำนวณนาที วินาที และมิลลิวินาที
        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);
        milliSeconds = Mathf.FloorToInt((timeToDisplay * 100) % 100);

        // 3. จัดรูปแบบ String (00:00:00)
        // :D2 คือการบังคับให้มีเลข 2 หลักเสมอ
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

    void UpdateStarDisplay(int starScore)
    {
        starText.text = string.Format("{0} / 10", starScore);
    }

    private void AddStar()
    {
        if (starScore < 10)
        {
            starScore++;
        }
    }

    private void RestartCounter()
    {
        elapsedTime = 0f;
        starScore = 0;
        isRunning = true;
    }

    private void StopCounter()
    {
        isRunning = false;
    }

    private void CompleteStage()
    {
        StopCounter();
        SaveTimeScore();
        SaveStarScore();
    }

    private void SaveTimeScore()
    {
        // Save best time
        if (PlayerPrefs.HasKey(bestTimePrefName))
        {
            float bestTime = PlayerPrefs.GetFloat(bestTimePrefName);
            if (elapsedTime < bestTime)
            {
                PlayerPrefs.SetFloat(bestTimePrefName, elapsedTime);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(bestTimePrefName, elapsedTime);
        }
    }

    private void SaveStarScore()
    {
        // Save best score
        if (PlayerPrefs.HasKey(bestScorePrefName))
        {
            int bestScore = PlayerPrefs.GetInt(bestScorePrefName);
            if (starScore > bestScore)
            {
                PlayerPrefs.SetInt(bestScorePrefName, starScore);
            }
        }
        else
        {
            PlayerPrefs.SetInt(bestScorePrefName, starScore);
        }
    }

    private void OnDestroy()
    {
        PlayerScripts.Instance.OnDeath -= StopCounter; // ยกเลิกการสมัครเมื่อไม่ใช้งาน
        PlayerScripts.Instance.OnCollectStar -= AddStar; // ยกเลิกการสมัครเมื่อไม่ใช้งาน
        GameManager.Instance.RestartEvent -= RestartCounter; // ยกเลิกการสมัครเมื่อไม่ใช้งาน
        GameManager.Instance.WinEvent -= CompleteStage;
    }
}
