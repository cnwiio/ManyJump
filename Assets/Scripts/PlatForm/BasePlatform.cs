using UnityEngine;

public class BasePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Bounce();
        }
    }

    protected virtual void Bounce()
    {
        PlayerScripts.Instance.Jump();
    }
}
