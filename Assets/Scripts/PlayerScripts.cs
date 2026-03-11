using System;
using System.Collections;
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
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float MaxFallSpeed = 10f;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;

    private float dir;

    private Vector3 leftSide;
    private Vector3 rightSide;

    private float loseOffset = 10f;
    private float loseYPos;

    private bool IsDead;
    public event Action OnDeath;

    private bool isFlying = false;
    private float initialGravityScale;
    private bool _hasShield = false;
    private bool hasShield
    {
        get => _hasShield;
        set
        {
            _hasShield = value;
            shieldVisual.SetActive(value);
        }
    }
    [Header("PowerUp")]
    [SerializeField] private GameObject shieldVisual;

    void Awake()
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftSide = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        rightSide = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
        loseYPos = transform.position.y - loseOffset;
        initialGravityScale = rb.gravityScale;
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

        AudioManager.Instance.PlaySFX(jumpSound);
    }

    public void Jump(float BouceForce)
    {
        if (rb.linearVelocityY > 0) return;
        rb.linearVelocityY = 0f;
        rb.AddForce(Vector2.up * BouceForce, ForceMode2D.Impulse);

        AudioManager.Instance.PlaySFX(jumpSound);
    }


    private void ClampFallSpeed()
    {
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -MaxFallSpeed);
        //Debug.Log($"Player Y Velocity: {rb.linearVelocityY}");
    }

    private void HorizontalMove()
    {
        rb.linearVelocityX = dir * horizontalSpeed;
        rb.linearVelocityX = Input.GetAxis("Horizontal") * horizontalSpeed;
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
            AudioManager.Instance.PlaySFX(deathSound);
            Die();
        }

        if (rb.linearVelocityY > 0)
        {
            loseYPos = transform.position.y - loseOffset;
        }
    }

    #region PowerUp


    public void ApplyFly(float flyDuration, float flySpeed)
    {
        isFlying = true;
        StartCoroutine(FlyCouroutine(flyDuration, flySpeed));
    }

    private IEnumerator FlyCouroutine(float duration, float speed)
    {
        rb.linearVelocityY = 0f;
        rb.gravityScale = 0f;
        rb.linearVelocityY = speed;
        yield return new WaitForSeconds(duration);
        rb.gravityScale = initialGravityScale;
        isFlying = false;
    }

    public void ApplyShield(float duration)
    {
        hasShield = true;
        StartCoroutine(ShieldCoroutine(duration)); 
    }

    private IEnumerator ShieldCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        hasShield = false;
    }

    #endregion

    #region Die
    public void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    public void HitEnemy()
    {
        if (hasShield)
        {
            hasShield = false;
            return;
        }
        Die();
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
