using UnityEngine;

public class FakePlatform : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D col;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.transform. position.y > transform.position.y)
                Bounce();
        }
    }

    protected void Bounce()
    {
        animator.SetTrigger("Break");
    }
}
