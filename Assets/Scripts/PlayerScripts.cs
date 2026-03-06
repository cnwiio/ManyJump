using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Lean.Pool;
using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    public static PlayerScripts Instance { get; private set; }
    private Rigidbody2D rb;
    [Header("Player Stats")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float MaxFallSpeed = 10f;

    private float dir;

    private Vector3 leftSide;
    private Vector3 rightSide;

    private float loseOffset = 10f;
    private float loseYPos;

    private bool IsDead;
    public event Action OnDeath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftSide = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        rightSide = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
        loseYPos = transform.position.y - loseOffset;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsDead) return;
        // Direction cal
        dir = Math.Clamp(Input.acceleration.x, -1, 1);
    }

    private void FixedUpdate()
    {
        ClampFallSpeed();
        HorizontalMove();
        CheckFall();
    }

    #region Movement
    public void Jump()
    {
        if (rb.linearVelocityY > 0) return;
        rb.linearVelocityY = 0f;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Jump(float BouceForce)
    {
        if (rb.linearVelocityY > 0) return;
        rb.linearVelocityY = 0f;
        rb.AddForce(Vector2.up * BouceForce, ForceMode2D.Impulse);
    }

    private void ClampFallSpeed()
    {
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -MaxFallSpeed);
        //Debug.Log($"Player Y Velocity: {rb.linearVelocityY}");
    }

    private void HorizontalMove()
    {
        rb.linearVelocityX = dir * speed;
        rb.linearVelocityX = Input.GetAxis("Horizontal") * speed;
        HandleOffScreen();
    }
    private void HandleOffScreen()
    {
        if (rb.position.x < leftSide.x)
        {
            rb.position = new Vector2(rightSide.x, rb.position.y);
        }

        if (rb.position.x > rightSide.x)
        {
            rb.position = new Vector2(leftSide.x, rb.position.y);
        }
    }

    #endregion

    private void CheckFall()
    {
        if (IsDead) return;

        if (transform.position.y < loseYPos)
        {
            Debug.Log("You Lose!");
            Die();
        }

        if (rb.linearVelocityY > 0)
        {
            loseYPos = transform.position.y - loseOffset;
        }
    }

    #region Die
    public void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    public void DieAndDisappear()
    {

    }

    public void Restart()
    {
        IsDead = false;
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        loseYPos = transform.position.y - loseOffset;
    }
    #endregion
}
