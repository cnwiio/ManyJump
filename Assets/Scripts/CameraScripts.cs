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
        Debug.Log("MoveDown");
        isMoveDown = true;
    }

    private void MoveDown()
    {
        if (!isMoveDown) return;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3 (transform.position.x, yHightestPos - DeadScreenYOffset, transform.position.z), Speed);

        if (transform.position.y == yHightestPos - DeadScreenYOffset)
        {
            isMoveDown = false;
        }
    }
    #endregion
}
