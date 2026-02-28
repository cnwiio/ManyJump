using Lean.Pool;
using UnityEngine;

public class BreakPlatform : BasePlatform
{
    private BoxCollider2D col;
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    protected override void Bounce()
    {
        base.Bounce();
        LeanPool.Despawn(this);
    }
}
