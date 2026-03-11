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
        Debug.Log("Break called");
        animator.SetTrigger("Break");
    }

    public void OnAnimationEnd()
    {
        Debug.Log("OnAnimationEnd called");
        LeanPool.Despawn(this);
    }

    public override void OnSpawn()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        Debug.Log("Reset called");
        animator.SetTrigger("Reset");
    }
}
