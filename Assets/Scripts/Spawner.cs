using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public sealed class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Preset> _presetLists;
    [SerializeField] private Preset[] _monAndPowerPresetLists;
    [SerializeField] private float _difficultyChangeTargetY;

    [Header("Spawn Distance")]
    [Tooltip("ระยะห่างจากตัวผู้เล่นที่เริ่มจะ Spawn ชิ้นใหม่ (ควรมากกว่าความสูงหน้าจอ)")]
    [SerializeField] private float _spawnAheadDistance = 15f;

    private GameObject _player;
    private float _targetY;
    private bool _hasAddedMonAndPower = false;

    private void Start()
    {
        if (PlayerScripts.Instance != null)
            _player = PlayerScripts.Instance.gameObject;

        if (GameManager.Instance != null)
            GameManager.Instance.RestartEvent += Restart;

        StartSpawn();
    }

    private void Update()
    {
        if (_player == null) return;

        // --- จุดสำคัญ: ตรวจสอบระยะห่างแทนการรอให้ถึงจุด ---
        // ถ้าตำแหน่ง Y ของผู้เล่น + ระยะที่กำหนด มากกว่าจุดสิ้นสุดของ Preset ล่าสุด
        // ให้ทำการ Spawn เพิ่มล่วงหน้าทันที
        if (_player.transform.position.y + _spawnAheadDistance > _targetY)
        {
            Spawn();
        }

        // ส่วนของการเพิ่มความยากยังคงเดิม
        if (_player.transform.position.y > _difficultyChangeTargetY && !_hasAddedMonAndPower)
        {
            AddMonAndPowerPresets();
        }
    }

    private void Spawn()
    {
        if (_presetLists == null || _presetLists.Count == 0) return;

        int randomIndex = Random.Range(0, _presetLists.Count);
        Preset selectedPreset = _presetLists[randomIndex];

        // วางที่จุด _targetY ปัจจุบัน
        Vector3 spawnBasePos = new Vector3(0f, _targetY, 0f);

        for (int i = 0; i < selectedPreset.transform.childCount; i++)
        {
            if (i < selectedPreset.ChildType.Length && i < selectedPreset.ChildPos.Length)
            {
                LeanPool.Spawn(selectedPreset.ChildType[i],
                    selectedPreset.ChildPos[i].localPosition + spawnBasePos,
                    Quaternion.identity);
            }
        }

        // เลื่อนจุด Target ไปที่ส่วนหัวของ Preset ที่เพิ่งวางเสร็จ
        _targetY += selectedPreset.Hieght;
    }

    private void StartSpawn()
    {
        // เริ่มต้นจากจุดที่ต่ำกว่าผู้เล่นเล็กน้อยเพื่อให้มีพื้นยืน
        _targetY = -10f;

        // Spawn ล่วงหน้าไว้สัก 3-4 ชิ้น เพื่อให้ฉากดูเต็มตั้งแต่เริ่ม
        for (int i = 0; i < 4; i++)
        {
            Spawn();
        }
    }

    private void AddMonAndPowerPresets()
    {
        if (_hasAddedMonAndPower) return;
        _presetLists.AddRange(_monAndPowerPresetLists);
        _hasAddedMonAndPower = true;
    }

    private void Restart()
    {
        if (_hasAddedMonAndPower)
        {
            foreach (var item in _monAndPowerPresetLists)
            {
                _presetLists.Remove(item);
            }
            _hasAddedMonAndPower = false;
        }
        StartSpawn();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartEvent -= Restart;
    }
}