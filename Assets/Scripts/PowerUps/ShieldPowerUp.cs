using UnityEngine;
using Lean.Pool;

public class ShieldPowerUp : BasePowerUp
{
    [SerializeField] private float shieldDuration = 5f;
    
    protected override void ApplyPowerUp()
    {
        PlayerScripts.Instance.ApplyShield(shieldDuration);
    }
}
