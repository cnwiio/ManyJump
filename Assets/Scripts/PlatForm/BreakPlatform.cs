using Lean.Pool;
using UnityEngine;

public class BreakPlatform : BasePlatform
{
    protected override void Bounce()
    {
        base.Bounce();
        LeanPool.Despawn(this);
    }
}
