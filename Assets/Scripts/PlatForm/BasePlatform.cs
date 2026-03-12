using Lean.Pool;
using UnityEngine;

public class BasePlatform : MonoBehaviour, IPoolable
{
    private float playerOffset = 1f;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            LeanPool.Despawn(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.transform.position.y - playerOffset > transform.position.y)
        {
            if (PlayerScripts.Instance.IsFalling)
                Bounce();
        }
    }

    protected virtual void Bounce()
    {
        PlayerScripts.Instance.Jump();
    }

    public virtual void OnSpawn()
    {
        //gameObject.SetActive(true);
    }

    public virtual void OnDespawn()
    {
        //gameObject.SetActive(false);
    }
}
