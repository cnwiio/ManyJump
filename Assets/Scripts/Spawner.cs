using UnityEngine;
using Lean.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Preset[] _presetLists;
    [SerializeField] private Vector3 offset;
    private GameObject _player;
    private float Targety;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = PlayerScripts.Instance.gameObject;
        Targety = -40;
        Spawn();
        Targety = -20;
        Spawn();
        Targety = 0;
        Spawn();
        Targety = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.y > Targety)
        {
            Spawn();
            Targety += offset.y;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    private Vector3 RandomPos;
    private int randomIndex;
    void Spawn()
    {
        randomIndex = Random.Range(0, _presetLists.Length);
        //Debug.Log(randomIndex);
        RandomPos = new Vector3(/*Random.Range(-2f, 2f)*/0f, Targety + offset.y, 0f);

        for (int i = 0; i < _presetLists[randomIndex].transform.childCount; i++)
        {
            LeanPool.Spawn(_presetLists[randomIndex].ChildType[i],
                _presetLists[randomIndex].ChildPos[i].localPosition + RandomPos,
                Quaternion.identity);
        }
    }
}
