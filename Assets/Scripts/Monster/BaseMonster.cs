using Lean.Pool;
using UnityEngine;

public class BaseMonster : MonoBehaviour, IPoolable
{
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.transform.position.y - 1 > transform.position.y)
        {
            Bounce();
        }
        else if (collision.gameObject.CompareTag("Player") &&
                   collision.gameObject.transform.position.y - 1 < transform.position.y)
        {
            Kill();
        }
    }

    public virtual void Bounce()
    {
        PlayerScripts.Instance.Jump();
    }

    public virtual void Kill()
    {
        PlayerScripts.Instance.HitEnemy();
    }

    public virtual void Die()
    {
        LeanPool.Despawn(gameObject);
    }

    public virtual void OnSpawn() 
    {
        
    }

    public virtual void OnDespawn()
    {

    }
}
