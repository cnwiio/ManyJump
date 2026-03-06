using UnityEngine;
using Lean.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Preset[] _presetLists;
    private GameObject _player;
    private float Targety;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = PlayerScripts.Instance.gameObject;
        GameManager.Instance.RestartEvent += Restart;
        StartSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.y > Targety)
        {
            Spawn();
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    #region Spawn
    private Vector3 RandomPos;
    private int randomIndex;
    private int previousHeight = 0;
    void Spawn()
    {
        randomIndex = Random.Range(0, _presetLists.Length);
        //Debug.Log(randomIndex);
        RandomPos = new Vector3(/*Random.Range(-2f, 2f)*/0f, Targety + previousHeight, 0f);

        //Debug.Log("Currect i : " + i + 
        //    "\nChild Type : " + _presetLists[randomIndex].ChildType.Length +
        //    "\nChildPos : " + _presetLists[randomIndex].ChildPos.Length + 
        //    "\nChild Cout : " + _presetLists[randomIndex].transform.childCount);
        for (int i = 0; i < _presetLists[randomIndex].transform.childCount; i++)
        {
            LeanPool.Spawn(_presetLists[randomIndex].ChildType[i],
                _presetLists[randomIndex].ChildPos[i].localPosition + RandomPos,
                Quaternion.identity);
        }

        previousHeight = (int)_presetLists[randomIndex].Hieght;
        Targety += _presetLists[randomIndex].Hieght;
    }

    void StartSpawn()
    {
        Targety = -10;
        Spawn();
        Targety = -10;
        Spawn();
    }
    #endregion
    private void Restart()
    {
        previousHeight = 0;
        StartSpawn();
    }
    private void OnDestroy()
    {
        GameManager.Instance.RestartEvent -= StartSpawn;
    }
}
