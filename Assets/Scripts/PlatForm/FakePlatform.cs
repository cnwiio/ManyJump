using UnityEngine;

public class FakePlatform : BasePlatform
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

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

    public override void OnSpawn()
    {
        base.OnSpawn();
        animator.SetTrigger("Reset");
    }
}
