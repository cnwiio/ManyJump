using NUnit.Framework.Interfaces;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    public static PlayerScripts Instance { get; private set; }
    private Rigidbody2D rb;
    [Header("Player Stats")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float MaxFallSpeed = 10f;

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
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump();
        }
    }

    private void FixedUpdate()
    {
        HorizontalMove();
        ClampFallSpeed();
        HandleOffScreen();
    }

    public void Jump()
    {
        if (rb.linearVelocityY > 0) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ClampFallSpeed()
    {
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -MaxFallSpeed);
        //Debug.Log($"Player Y Velocity: {rb.linearVelocityY}");
    }

    private void HorizontalMove()
    {
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.linearVelocityY);
    }

    [Header("Off Screen Handling")]
    [SerializeField] float leftSide = -10f;
    [SerializeField] float rightSide = 10f;
    private void HandleOffScreen()
    {
        if (rb.position.x < leftSide)
        {
            rb.position = new Vector2(rightSide, rb.position.y);
        }

        if (rb.position.x > rightSide)
        {
            rb.position = new Vector2(leftSide, rb.position.y); 
        }
    }
}
