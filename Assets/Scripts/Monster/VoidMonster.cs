using UnityEngine;

public class VoidMonster : BaseMonster
{
    public override void Bounce()
    {
    }
    public override void Kill()
    {
        PlayerScripts.Instance.HitEnemy();
    }
}
