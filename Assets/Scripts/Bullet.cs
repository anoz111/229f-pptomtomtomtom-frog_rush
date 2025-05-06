using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // ลบกระสุนหลังผ่านไป 3 วิ
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // ตรวจว่า collider ที่โดนคือศัตรู
        {
            FrogEnemy frog = other.GetComponent<FrogEnemy>();
            if (frog != null)
            {
                frog.TakeDamage(damage); // ทำดาเมจ 1
            }

            Destroy(gameObject); // ทำลายกระสุนหลังชน
        }
    }
}
