using Lean.Pool;
using UnityEngine;

public class BaseMonster : MonoBehaviour, IPoolable
{
    [SerializeField] private RuntimeAnimatorController[] variantsAnimatorList;
    [SerializeField] private RuntimeAnimatorController[] SpaceAnimatorList;
    private Animator animator;
    private int TargetSpace = 40;
    private bool onSpace;

    private void Start()
    {
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.transform.position.y - 1 > transform.position.y)
        {
            Bounce();
        }
        else if (collision.gameObject.CompareTag("Player") &&
                   collision.gameObject.transform.position.y - 1 < transform.position.y)
        {
            Kill();
        }
    }

    public virtual void Bounce()
    {
        PlayerScripts.Instance.Jump();
    }

    public virtual void Kill()
    {
        PlayerScripts.Instance.HitEnemy();
    }

    public virtual void Die()
    {
        LeanPool.Despawn(gameObject);
    }

    int randomIndex;
    public virtual void OnSpawn() 
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (gameObject.transform.position.y > TargetSpace && SpaceAnimatorList.Length != 0 && !onSpace)
        {
            onSpace = true;
            variantsAnimatorList[0] = SpaceAnimatorList[0];
        }
        
        randomIndex = Random.Range(0, variantsAnimatorList.Length);
        animator.runtimeAnimatorController = variantsAnimatorList[randomIndex];
    }

    public virtual void OnDespawn()
    {

    }
}
