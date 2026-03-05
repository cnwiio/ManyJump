using UnityEngine;
using Lean.Pool;

public class BombPlatform : BasePlatform
{
    private Animator animator;
    private bool isBreaking = false;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (collision.gameObject.transform.position.y > transform.position.y)
    //            Break();
    //    }
    //}

    protected override void Bounce()
    {
        base.Bounce();
        if (isBreaking) return;
        animator.SetTrigger("Break");
        isBreaking = true;
    }

    public void OnAnimationEnd()
    {
        LeanPool.Despawn(this);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetTrigger("Reset");
        isBreaking = false;
    }
}
