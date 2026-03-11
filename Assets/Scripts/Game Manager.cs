using System;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action RestartEvent;
    public event Action WinEvent;


    [SerializeField] private GameObject RestartCanvas;
    [SerializeField] private GameObject WinCanvas;
    [SerializeField] private GameObject PauseCanvas;

    private bool isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerScripts.Instance.OnDeath += Lose;
        PlayerScripts.Instance.OnTouchWin += Win;
    }

    // Update is called once per frame
    void Update()
    {
        //if(PlayerScripts.Instance.transform.position.y < LoseYpos)
        //{

        //}
    }

    public void Lose()
    {
        PlayerScripts.Instance.Die();
        RestartCanvas.SetActive(true);
    }

    public void Win()
    {
        WinEvent?.Invoke();
        WinCanvas.SetActive(true);
        UnlockNextStage();
    }

    public void Restart()
    {
        AudioManager.Instance.PlayClick();

        Time.timeScale = 1f;
        isPaused = false;

        LeanPool.DespawnAll();
        PlayerScripts.Instance.Restart();
        RestartEvent?.Invoke();

        RestartCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
    }

    int currentStage = 1;
    int nxtStage;
    private void UnlockNextStage()
    {
        nxtStage = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.HasKey("CurrentStage"))
        {
            currentStage = PlayerPrefs.GetInt("CurrentStage");
            if (currentStage <= SceneManager.GetActiveScene().buildIndex - 1)
            {
                PlayerPrefs.SetInt("CurrentStage", nxtStage);
            }
            //Debug.Log(currentStage + "\n" +  (SceneManager.GetActiveScene().buildIndex-1));
        }
        else
        {
            PlayerPrefs.SetInt("CurrentStage", nxtStage);
        }
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlayClick();

        isPaused = true;
        Time.timeScale = 0f;
        PauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlayClick();

        isPaused = false;
        Time.timeScale = 1f;
        RestartCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
    }
    public void Home()
    {
        AudioManager.Instance.PlayClick();

        isPaused = false;
        Time.timeScale = 1f;
        RestartCanvas.SetActive(false);
        PauseCanvas.SetActive(false);

        //FindObjectOfType<SceneManagement>().LoadLevel(0);
    }

    private void OnDestroy()
    {
        PlayerScripts.Instance.OnDeath -= Lose;
        PlayerScripts.Instance.OnTouchWin -= Win;
    }
}
