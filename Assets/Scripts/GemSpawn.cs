using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject gemPrefab; // พรีแฟบของเหรียญ (หรือไอเทม)
    [SerializeField] Transform[] spawnPoints; // จุดที่จะสุ่มสร้างเหรียญ
    [SerializeField] float spawnInterval = 2f; // เวลาระหว่างการสุ่มสร้าง

    float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime; // ลดเวลาลง

        if (timer <= 0f) // ถ้าถึงเวลาที่กำหนด
        {
            SpawnCoin(); // สร้างเหรียญ
            timer = spawnInterval; // รีเซ็ตเวลา
        }
    }

    void SpawnCoin()
    {
        if (spawnPoints.Length == 0) return;

        int randIndex = Random.Range(0, spawnPoints.Length); // เลือกตำแหน่งสุ่ม
        Transform spawnPoint = spawnPoints[randIndex];

        Instantiate(gemPrefab, spawnPoint.position, Quaternion.identity); // สร้างเหรียญ
    }
}

