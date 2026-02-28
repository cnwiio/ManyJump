using UnityEngine;
using Lean.Pool;

public class FakePlatform : BasePlatform
{
    private Animator animator;
    //void Start()
    //{
    //    //animator = GetComponent<Animator>();
    //}

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
        animator.SetTrigger("Break");
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
    }
}
