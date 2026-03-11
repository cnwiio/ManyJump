using UnityEngine;

public class VoidMonster : BaseMonster
{
    public override void Kill()
    {
        PlayerScripts.Instance.HitEnemy();
    }
}
