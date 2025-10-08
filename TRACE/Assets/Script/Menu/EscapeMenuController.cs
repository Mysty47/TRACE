using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuController : MonoBehaviour
{
    [SerializeField] 
    private GameObject pauseMenu;
    public static bool isPaused = false;

    void Start()
    {
        ResumeGame(); // Ensure game starts unpaused
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
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        isPaused = false;
        // ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        isPaused = false;
        ResumeGame();
        SceneManager.LoadScene(0);
    }
}