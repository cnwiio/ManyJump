using Lean.Pool;
using UnityEngine;

public abstract class BasePowerUp : MonoBehaviour, IPoolable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyer"))
        {
            LeanPool.Despawn(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            if (PlayerScripts.Instance.IsHavePowerUp()) return;
            ApplyPowerUp();
            Die();
        }
    }

    protected virtual void ApplyPowerUp()
    {
        // This method should be overridden by derived classes to implement specific power-up behavior.
    }

    protected virtual void Die()
    {
        LeanPool.Despawn(gameObject);
    }

    public void OnSpawn()
    {
        // This method can be used to initialize the power-up when it is spawned from the pool.
    }

    public void OnDespawn()
    {
        // This method can be used to clean up the power-up when it is returned to the pool.
    }
}
