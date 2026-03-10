using System;
using Lean.Pool;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action RestartEvent;

    [SerializeField] private GameObject RestartCanvas;

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
        LeanPool.DespawnAll();
        PlayerScripts.Instance.Restart();
        RestartEvent?.Invoke();

        RestartCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerScripts.Instance.OnDeath -= Lose;
    }

}
