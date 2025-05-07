using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Text scoreText; 
    private int finalScore;

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + finalScore;
        }
    }
    public void ShowGameOver(int score)
    {
        UpdateScoreDisplay();
        finalScore = score;
        scoreText.text = "Score : " + finalScore;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // หยุดเวลา
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}