using System;
using Lean.Pool;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action RestartEvent;


    [SerializeField] private GameObject RestartCanvas;
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

        if (PauseCanvas != null)
        {
            PauseCanvas.SetActive(false);
        }
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
        RestartCanvas.SetActive(true);
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

        FindObjectOfType<SceneManagement>().LoadLevel(0);
    }

    private void OnDestroy()
    {
        PlayerScripts.Instance.OnDeath -= Lose;
    }
}