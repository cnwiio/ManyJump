using System.Collections.Generic;
using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Preset> _presetLists;
    [SerializeField] private Preset[] _MonAndPowerPresetLists;
    private GameObject _player;
    private float Targety;
    [SerializeField] private float diffucultChangeTargetY;
    private bool _hasAddedMonAndPower = false;
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

        if (_player.transform.position.y > diffucultChangeTargetY && !_hasAddedMonAndPower)
        {
            AddMonAndPowerPresets();
            _hasAddedMonAndPower = true;
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
        randomIndex = Random.Range(0, _presetLists.Count);
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

    void AddMonAndPowerPresets()
    {
        if (_hasAddedMonAndPower) return;
        _presetLists.AddRange(_MonAndPowerPresetLists);
        Debug.Log(_presetLists.Count);
    }
    #endregion
    private void Restart()
    {
        previousHeight = 0;
        _presetLists.RemoveRange(_presetLists.Count - _MonAndPowerPresetLists.Length, _MonAndPowerPresetLists.Length);
        _hasAddedMonAndPower = false;
        StartSpawn();
    }
    private void OnDestroy()
    {
        GameManager.Instance.RestartEvent -= Restart;
    }
}
