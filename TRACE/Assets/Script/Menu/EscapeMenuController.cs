using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Serialization;

public class EscapeMenuController : MonoBehaviour
{
    [SerializeField] 
    private GameObject pauseMenu;
    public static bool isPaused = false;
    private EventSystem myEventSystem;
    
    [Header("References")]
    public PlayerHealth playerHealth;

    void Awake()
    {
        myEventSystem = EventSystem.current;
    }

    void Start()
    {
        ResumeGame(); // Ensure game starts unpaused
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !playerHealth.isDead)
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
            StartCoroutine(DeselectAfterFrame());
        }
        else
        {
            ResumeGame();
            StartCoroutine(DeselectAfterFrame());
        }
    }

    /* Deselects every object in EventSystem at the end of the frame
     so Unity doesn't have time to select them again
     */
    private IEnumerator DeselectAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        if (myEventSystem != null)
        {
            myEventSystem.SetSelectedGameObject(null);
        }
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        isPaused = false;
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}