using UnityEngine;

public class SuperBouncePlatform : BasePlatform
{
    [SerializeField] float BounceForce = 20f;
    [SerializeField] Animator animator;
    protected override void Bounce()
    {
        PlayerScripts.Instance.Jump(BounceForce);
        if (animator != null) animator.SetTrigger("Bounce");
    }
}
