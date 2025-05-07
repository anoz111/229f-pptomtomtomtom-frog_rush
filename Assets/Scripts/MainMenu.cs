using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
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
