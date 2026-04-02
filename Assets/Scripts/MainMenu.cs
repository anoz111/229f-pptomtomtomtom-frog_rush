using UnityEngine;
using UnityEngine.SceneManagement;

// Analytics
using Unity.Services.Analytics;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainStage");
    }

    public void ShowCredit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        // ✅ ยิง event: retry_game
        try
        {
            AnalyticsService.Instance.RecordEvent("retry_game");
            Debug.Log("SEND: retry_game");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Retry Analytics Error: " + e.Message);
        }

        if (GameManager.Instance != null)
            GameManager.Instance.RestoreRunSnapshot();

        string lastStage = PlayerPrefs.GetString("LAST_STAGE", "MainStage");
        SceneManager.LoadScene(lastStage);
    }
}