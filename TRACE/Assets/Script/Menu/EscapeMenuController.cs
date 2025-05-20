using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;
    public Target target;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Resume the game
        }
    }

    public void restartLevel(int currentLevel)
    {
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(currentLevel);
    }
}