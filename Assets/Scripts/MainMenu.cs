using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // เปลี่ยนชื่อเป็น scene ที่คุณจะเล่นจริง
        SceneManager.LoadScene("SampleScene");
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
}
