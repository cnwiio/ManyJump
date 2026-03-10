using UnityEngine;
using Lean.Pool;

public class FlyPowerUp : BasePowerUp
{
    [SerializeField] private float flyDuration = 5f;
    [SerializeField] private float flySpeed = 1.5f;
    protected override void ApplyPowerUp()
    {
        PlayerScripts.Instance.ApplyFly(flyDuration, flySpeed);
    }
}
