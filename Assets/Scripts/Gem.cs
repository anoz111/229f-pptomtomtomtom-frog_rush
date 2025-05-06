using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] int coinValue = 1;  // จำนวนคะแนนที่ได้จากการเก็บเหรียญ
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AddScore(coinValue);  // เพิ่มคะแนนให้กับผู้เล่น
                Destroy(gameObject);  // ลบเหรียญออกจากเกม
            }
        }
    }
}
