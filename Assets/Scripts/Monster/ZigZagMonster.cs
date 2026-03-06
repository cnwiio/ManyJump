using UnityEngine;

public class ZigZagMonster : BaseMonster
{
    private Rigidbody2D rb;
    private float dir = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float offset = 1f;
    [SerializeField] private float moveRange = 1f;

    private Vector3 leftSide;
    private Vector3 rightSide;
    private float limitLeftSide; 
    private float limitRightSide; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftSide = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        rightSide = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
        limitLeftSide = transform.position.x - moveRange;
        limitRightSide = transform.position.x + moveRange;
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
        if (rb.position.x - offset < leftSide.x || rb.position.x < limitLeftSide)
        {
            dir = 1f;
        }

        if (rb.position.x + offset > rightSide.x || rb.position.x > limitRightSide)
        {
            dir = -1f;
        }
    }
}
