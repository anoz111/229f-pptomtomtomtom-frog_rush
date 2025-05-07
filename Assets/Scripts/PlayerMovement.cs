using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;

    [SerializeField] Slider hpSlider;

    float jumpCooldownTimer = 0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        
        UpdateScoreDisplay();
        UpdateHPBar();
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (jumpCooldownTimer > 0f)
            jumpCooldownTimer -= Time.deltaTime;

        
        if (Input.GetButtonDown("Jump") && jumpCooldownTimer <= 0f)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCooldownTimer = jumpCooldown; 
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

        UpdateHPBar();

        if (currentHP <= 0)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            Debug.Log("Player Died!");
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHP / maxHP;
        }
    }     
}
