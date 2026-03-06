using UnityEngine;

public class CameraScripts : MonoBehaviour
{
    private Transform targetTransform;
    private float yHightestPos;
    private bool isMoveDown;

    [Header("Camera Settings")]
    [SerializeField] private float DeadScreenYOffset;
    [SerializeField] private float Speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetTransform = PlayerScripts.Instance.transform;
        yHightestPos = targetTransform.position.y;
        transform.position = new Vector3(transform.position.x, yHightestPos, transform.position.z);
        GameManager.Instance.RestartEvent += Restart;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    private void FixedUpdate()
    {
        MoveDown();
    }

    void FollowTarget()
    {
        if (isMoveDown) return;
        if (targetTransform == null)
        {
            return;
        }

        if (yHightestPos < targetTransform.position.y)
        {
            yHightestPos = targetTransform.position.y;
            transform.position = new Vector3(transform.position.x, yHightestPos, transform.position.z);
        }
    }

    #region MoveDown
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartMoveDown();
        }
    }

    private void StartMoveDown()
    {
        isMoveDown = true;
    }

    private Vector3 loseScreenPos;
    private void MoveDown()
    {
        if (!isMoveDown) return;

        loseScreenPos = new Vector3(transform.position.x, yHightestPos - DeadScreenYOffset, transform.position.z);

        // Smoothly interpolate towards the target. 
        // Note: It's best practice to multiply Speed by Time.deltaTime here for frame-rate independence.
        transform.position = Vector3.Lerp(transform.position, loseScreenPos, Speed * Time.deltaTime);

        // Check if the Y position is close enough to the target (e.g., within 0.01 units)
        if (Mathf.Abs(transform.position.y - loseScreenPos.y) < 0.01f)
        {
            // Snap to the exact position to prevent micro-adjustments and end the movement
            transform.position = loseScreenPos;
            isMoveDown = false;
        }
    }
    #endregion

    private void Restart()
    {
        isMoveDown = false;
        yHightestPos = targetTransform.position.y;
        transform.position = new Vector3(transform.position.x, yHightestPos, transform.position.z);
    }

    private void OnDestroy()
    {
        GameManager.Instance.RestartEvent -= Restart;
    }
}
