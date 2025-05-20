using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [SerializeField] int levelToLoad;

    void OnTriggerEnter(Collider ChangeScene)
    {
        if(ChangeScene.gameObject.CompareTag("Player"))
        {
            if (levelToLoad == 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
        }
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
