using UnityEngine;

public class JumpPowerUp : BasePowerUp
{
    [SerializeField] float duration = 1f;
    [SerializeField] float multiply = 1f;
    protected override void ApplyPowerUp()
    {
        PlayerScripts.Instance.ApplyJumpPower(duration, multiply);
    }
}
