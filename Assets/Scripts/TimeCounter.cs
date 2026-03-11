using UnityEngine;
using TMPro; // จำเป็นสำหรับการใช้งาน TextMeshPro

public class TimeCounter : MonoBehaviour
{

    private float elapsedTime = 0f;
    public float TimeScore => elapsedTime; // ให้สามารถเข้าถึงเวลาที่ผ่านไปได้จากภายนอก
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return; // ถ้าไม่กำลังนับเวลา ให้ข้ามการอัปเดต

        // 1. สะสมเวลาที่ผ่านไปในแต่ละเฟรม
        elapsedTime += Time.deltaTime;
    }

    private void RestartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    private void StopTimer()
    {
        isRunning = false;
    }

    private void Start()
    {
        PlayerScripts.Instance.OnDeath += StopTimer; // รีเซ็ตเวลาเมื่อผู้เล่นตาย
        GameManager.Instance.RestartEvent += RestartTimer; // รีเซ็ตเวลาเมื่อเกมรีสตาร์ท
    }
    
    private void OnDestroy()
    {
        PlayerScripts.Instance.OnDeath -= StopTimer; // ยกเลิกการสมัครเมื่อไม่ใช้งาน
        GameManager.Instance.RestartEvent -= RestartTimer; // ยกเลิกการสมัครเมื่อไม่ใช้งาน
    }

}