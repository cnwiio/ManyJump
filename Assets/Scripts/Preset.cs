using UnityEngine;
using Lean.Pool;

public enum PlatformType
{
    Normal,
    PingPong,
    SuperBounce,
    Break,
    Fake
}

public class Preset : MonoBehaviour, IPoolable
{
    public float Hieght = 1f;
    public Transform[] ChildPos;
    public GameObject[] ChildType;
    //[SerializeField] private GameObject[] platformPrefabs;
    void Update()
    {
        //if (transform.childCount < 1)
        //{
        //    LeanPool.Despawn(gameObject);
        //}
        //Debug.Log(transform.childCount);
    }

    public void OnSpawn()
    {
        // This method is called when the object is spawned from the pool.
        // You can initialize any necessary variables or states here.
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        // This method is called when the object is despawned back into the pool.
        // You can reset any variables or states here to prepare the object for reuse.
        gameObject.SetActive(false);
    }
}
