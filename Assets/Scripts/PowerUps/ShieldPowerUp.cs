using UnityEngine;
using Lean.Pool;

public class ShieldPowerUp : BasePowerUp
{
    [SerializeField] private float shieldDuration = 5f;
    
    protected override void ApplyPowerUp()
    {
        if (PlayerScripts.Instance.hasShield) return;
        PlayerScripts.Instance.ApplyShield(shieldDuration);
    }

    protected override void Die()
    {
        if (PlayerScripts.Instance.hasShield) return;
        base.Die();
    }
}
