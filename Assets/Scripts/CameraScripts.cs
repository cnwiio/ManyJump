using UnityEngine;

public class CameraScripts : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private float yHightestPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yHightestPos = targetTransform.position.y;
        transform.position = new Vector3(transform.position.x, yHightestPos, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (yHightestPos < targetTransform.position.y)
        {
            yHightestPos = targetTransform.position.y;
        }

        transform.position = new Vector3(transform.position.x, yHightestPos, transform.position.z);
    }
}
