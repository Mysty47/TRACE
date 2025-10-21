using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string _newGameLevel;
    private string levelToLoad;

    public void StartLevel()
    {
        EscapeMenuController.isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(_newGameLevel);
    }

    public void StartGame()
    {
        EscapeMenuController.isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void StartEndless()
    {
        EscapeMenuController.isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(5);
    }

    public void SelectLevel(string SelectLevelName)
    {
        EscapeMenuController.isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SelectLevelName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
