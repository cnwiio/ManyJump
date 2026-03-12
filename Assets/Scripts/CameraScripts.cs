using UnityEngine;

public class CameraScripts : MonoBehaviour
{
    private Transform targetTransform;
    private float yHightestPos;
    private bool isMoveDown;

    [Header("Camera Settings")]
    [SerializeField] private float DeadScreenYOffset;
    [SerializeField] private float Speed;
    [Tooltip("ความกว้างของฉากที่คุณต้องการให้ผู้เล่นมองเห็นได้เสมอ (World Units)")]
    [SerializeField] private float sceneWidth = 10f; // สมมติว่าฉากกว้าง 10 unit

    private Camera _camera;
    private BoxCollider2D col;
    private Vector3 camBottom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetTransform = PlayerScripts.Instance.transform;
        yHightestPos = targetTransform.position.y;
        transform.position = new Vector3(transform.position.x, yHightestPos, transform.position.z);
        _camera = GetComponent<Camera>();
        col = GetComponent<BoxCollider2D>();
        AdjustCameraSize();
        camBottom = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        //Debug.Log(camBottom.y);

        GameManager.Instance.RestartEvent += Restart;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustCameraSize();

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
            if (isMoveDown) return;
            StartMoveDown();
            GameManager.Instance.Lose();
        }
    }

    private void StartMoveDown()
    {
        isMoveDown = true;
    }

    private Vector3 loseScreenPos;
    private float loseY;
    [SerializeField] private float limitScreenPoint;
    private void MoveDown()
    {
        if (!isMoveDown) return;

        if (yHightestPos - DeadScreenYOffset < limitScreenPoint)
        {
            loseY = limitScreenPoint - camBottom.y;
        }
        else
        {
            loseY = yHightestPos - DeadScreenYOffset;
        }

        loseScreenPos = new Vector3(transform.position.x, loseY, transform.position.z);

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

    /// <summary>
    /// คำนวณและปรับ Orthographic Size ของกล้องเพื่อให้ครอบคลุมความกว้างของฉากที่กำหนด
    /// </summary>
    private void AdjustCameraSize()
    {
        // สูตรการคำนวณ:
        // Orthographic Size = ความสูงของหน้าจอ / 2 (ใน World Units)
        // Aspect Ratio = ความกว้างของหน้าจอ / ความสูงของหน้าจอ
        // ดังนั้น, Orthographic Size = (ความกว้างของหน้าจอ / Aspect Ratio) / 2

        float currentAspectRatio = (float)Screen.width / Screen.height;

        // เป้าหมายของเราคือให้ความกว้างของหน้าจอ (Screen Width) มีขนาดเท่ากับ sceneWidth
        float desiredHalfWidth = sceneWidth / 2f;

        // ปรับ Orthographic Size:
        // Ortho Size = Half Width / Aspect Ratio
        float targetOrthoSize = desiredHalfWidth / currentAspectRatio;

        // กำหนดค่า Size ให้กับกล้อง
        _camera.orthographicSize = targetOrthoSize;
        col.offset = new Vector2(0, -_camera.orthographicSize - 0.5f);
    }

    private void OnDestroy()
    {
        GameManager.Instance.RestartEvent -= Restart;
    }
}
