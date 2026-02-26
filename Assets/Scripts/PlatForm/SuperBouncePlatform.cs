using UnityEngine;

public class SuperBouncePlatform : BasePlatform
{
    [SerializeField] float BounceForce = 20f;
    
    protected override void Bounce()
    {
        PlayerScripts.Instance.Jump(BounceForce);
    }
}
