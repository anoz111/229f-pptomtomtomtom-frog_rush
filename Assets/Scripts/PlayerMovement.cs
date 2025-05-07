using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // ใช้สำหรับ UI Slider

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpCooldown = 0.5f;
    [SerializeField] int maxHP = 3;
    int currentHP;

    [SerializeField] GameObject deathEffect;

    [SerializeField] TextMeshProUGUI scoreText;  // TextMeshPro สำหรับแสดงคะแนน
    public GameOverUI gameOverUI;
    int score = 0;

    [SerializeField] Slider hpSlider;  // ตัวแปรสำหรับแถบเลือด

    float jumpCooldownTimer = 0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        // เริ่มต้นแสดงคะแนนและแถบเลือด
        UpdateScoreDisplay();
        UpdateHPBar();
    }

    void Update()
    {
        // รับอินพุตแนวนอน
        moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);

        // นับถอยหลัง cooldown
        if (jumpCooldownTimer > 0f)
            jumpCooldownTimer -= Time.deltaTime;

        // กระโดดเมื่อ cooldown หมด
        if (Input.GetButtonDown("Jump") && jumpCooldownTimer <= 0f)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCooldownTimer = jumpCooldown; // เริ่มนับ cooldown ใหม่
            Debug.Log("Jump Leaw Ja!!");
        }
    }

    void FixedUpdate()
    {
        rb2d.linearVelocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log("Player hit! HP: " + currentHP);

        // อัปเดตแถบเลือด
        UpdateHPBar();

        if (currentHP <= 0)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            Debug.Log("Player Died!");
            Destroy(gameObject);  // ทำลายผู้เล่นเมื่อ HP เป็น 0
            SceneManager.LoadScene("GameOver");
        }
    }

    // ฟังก์ชันเพิ่มคะแนน
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        UpdateScoreDisplay();
    }

    // ฟังก์ชันอัปเดตคะแนน UI
    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // ฟังก์ชันอัปเดตแถบเลือด
    void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            // คำนวณค่าของ Slider โดยใช้ HP
            hpSlider.value = (float)currentHP / maxHP;
        }
    }
    public void GameOver()
    {
        gameOverUI.ShowGameOver(score);
    }    
}
