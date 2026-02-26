using UnityEngine;

public class PingPongPlatform : BasePlatform
{
    private Rigidbody2D rb;
    private float dir = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float offset = 1f;

    private Vector3 leftSide;
    private Vector3 rightSide;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftSide = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        rightSide = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
    }
    private void Update()
    {
        HandHandleOffScreen();
        MoveMent();
    }

    private void MoveMent()
    {
        rb.linearVelocityX = dir * speed;
    }

    private void HandHandleOffScreen()
    {
        if (rb.position.x - offset < leftSide.x)
        {
            dir = 1f;
        }

        if (rb.position.x + offset > rightSide.x)
        {
            dir = -1f;
        }
    }
}
