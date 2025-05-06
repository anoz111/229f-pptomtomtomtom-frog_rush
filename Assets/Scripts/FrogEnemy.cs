using UnityEngine;

public class FrogEnemy : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float jumpInterval = 2f; // เวลาระหว่างกระโดด
    [SerializeField] int maxHP = 1;
    int currentHP;

    float jumpTimer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        jumpTimer = jumpInterval;
        currentHP = maxHP;
    }

    void Update()
    {
        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0f)
        {
            JumpRandomDirection();
            jumpTimer = jumpInterval;
        }
    }

    void JumpRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f); // สุ่มแนวนอน
        Vector2 jumpDir = new Vector2(randomX, 1f).normalized;

        rb2d.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }
    public void TakeDamage(int amount)
{
    currentHP -= amount;
    Debug.Log("Frog took damage. HP: " + currentHP);

    if (currentHP <= 0)
    {
        Die();
    }
}

void Die()
{
    // เอฟเฟกต์หรือเสียงประกอบตรงนี้ก็ได้
    Destroy(gameObject);
}
    
}
