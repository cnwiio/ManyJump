using Lean.Pool;
using UnityEngine;

public class BasePlatform : MonoBehaviour, IPoolable
{
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            LeanPool.Despawn(gameObject);
        }

        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.transform.position.y - 1 > transform.position.y)
        {
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
