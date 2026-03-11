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
    [SerializeField] private bool pc = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip equitSound;
    [SerializeField] private AudioClip collectStarSound;

    [Header("Power")]
    [SerializeField] private float invincibleAfterHit = 3f;
    [SerializeField] private float flashInterval = 0.15f;
    private bool isInvincible = false;
    private Coroutine shieldRoutine;

    private float dir;

    private Vector3 leftSide;
    private Vector3 rightSide;

    private float loseOffset = 10f;
    private float loseYPos;

    private bool isDead;
    private bool isWin;
    public event Action OnDeath;

    public bool isFlying = false;
    private bool isSuperJump = false;
    private float initialGravityScale;
    private bool _hasShield = false;
    public bool hasShield
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

    public event Action OnCollectStar;

    public event Action OnTouchWin;


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
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        leftSide = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        rightSide = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
        loseYPos = transform.position.y - loseOffset;
        initialGravityScale = rb.gravityScale;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsDeadOrWin()) return;

        // Direction cal
        dir = Math.Clamp(Input.acceleration.x, -1, 1);
    }

    private void FixedUpdate()
    {
        ClampFallSpeed();
        HorizontalMove();
        CheckFall();
        CheckSuperJump();
    }

    #region Movement
    public void Jump()
    {
        if (IsDeadOrWin()) return;

        if (rb.linearVelocityY > 0) return;
        rb.linearVelocityY = 0f;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("Jump");
        AudioManager.Instance.PlaySFX(jumpSound);
    }

    public void Jump(float BouceForce)
    {
        if (IsDeadOrWin()) return;

        if (rb.linearVelocityY > 0 ) return;
        rb.linearVelocityY = 0f;
        rb.AddForce(Vector2.up * BouceForce, ForceMode2D.Impulse);

        animator.SetTrigger("Jump");
        AudioManager.Instance.PlaySFX(jumpSound);
    }


    private void ClampFallSpeed()
    {
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -MaxFallSpeed);
        //Debug.Log($"Player Y Velocity: {rb.linearVelocityY}");
    }

    private void HorizontalMove()
    {
        if (IsDeadOrWin()) return;
        rb.linearVelocityX = dir * horizontalSpeed;
        if (pc) rb.linearVelocityX = Input.GetAxis("Horizontal") * horizontalSpeed;
        FlipSprite();
        HandleOffScreen();
    }

    private void FlipSprite()
    {
        if (rb.linearVelocityX > 0)
        {
            spriteRenderer.flipX = false;
        } 
        else if (rb.linearVelocityX < 0)
        {
            spriteRenderer.flipX = true;
        }
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

    private void CheckSuperJump()
    {
        if (rb.linearVelocityY > jumpForce)
        {
            isSuperJump = true;
        } else
        {
            isSuperJump = false;
        }
    }

    #endregion

    private void CheckFall()
    {
        if (IsDeadOrWin()) return;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsDeadOrWin()) return;

        if (collision.CompareTag("Win"))
        {
            Winning();
        }
    }

    private void Winning()
    {
        OnTouchWin?.Invoke();
        isWin = true;
        //rb.linearVelocityY = 0;
    }

    #region PowerUp

    IEnumerator InvincibleCoroutine(float duration)
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    public void ApplyFly(float flyDuration, float flySpeed)
    {
        if (IsDeadOrWin()) return;
        if (IsHavePowerUp()) return;
        isFlying = true;
        StartCoroutine(FlyCouroutine(flyDuration, flySpeed));
        AudioManager.Instance.PlaySFX(equitSound);
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
        if (IsDeadOrWin()) return;
        if (IsHavePowerUp()) return;
        hasShield = true;

        shieldRoutine = StartCoroutine(ShieldCoroutine(duration));

        AudioManager.Instance.PlaySFX(equitSound);
    }

    private IEnumerator ShieldCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration - 1f);

        for (int i = 0; i < 6; i++)
        {
            shieldVisual.SetActive(!shieldVisual.activeSelf);
            yield return new WaitForSeconds(0.15f);
        }

        hasShield = false;
    }

    public bool IsHavePowerUp()
    {
        return hasShield || isSuperJump || isFlying;
    }

    #endregion

    #region Score

    public void CollectStar()
    {
        if (IsDeadOrWin()) return;
        OnCollectStar?.Invoke();
        AudioManager.Instance.PlaySFX(collectStarSound);
    }

    #endregion

    #region Die
    public void Die()
    { 
        AudioManager.Instance.PlaySFX(deathSound);
        isDead = true;
        OnDeath?.Invoke();
    }

    public void HitEnemy() { 
        if (isInvincible) return;

        if (IsDeadOrWin()) return;
        if (isFlying || isSuperJump) return;
    
        if (hasShield)
        {
            hasShield = false;

            if (shieldRoutine != null)
            {
                StopCoroutine(shieldRoutine);
            }

            shieldVisual.SetActive(false);

            StartCoroutine(InvincibleCoroutine(invincibleAfterHit));
            return;
        }

        Die();
    }
    public void Restart()
    {
        isDead = false;
        isFlying = false;
        isWin = false;
        isSuperJump = false;
        hasShield = false;
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        loseYPos = transform.position.y - loseOffset;
    }

    public bool IsDeadOrWin()
    {
        return isDead || isWin;
    }
    #endregion
}
