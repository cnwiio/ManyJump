using UnityEngine;

public class Star : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isCollected = false;

    public void Start()
    {
        GameManager.Instance.RestartEvent += Spawn;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Spawn()
    {
        spriteRenderer.enabled = true;
        isCollected = false;
    }

    public void Collect()
    {
        if (isCollected) return;    
        isCollected = true;
        PlayerScripts.Instance.CollectStar();
        Despawn();
    }

    public void Despawn()
    {
        spriteRenderer.enabled = false;
    }

    public void OnDestroy()
    {
        GameManager.Instance.RestartEvent -= Spawn;
    }
}
