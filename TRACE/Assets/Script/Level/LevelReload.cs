using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelReload : MonoBehaviour
{
    public Target t;
    public string newGameLevelWhenDead;
    
    void OnTriggerEnter(Collider ChangeScene)
    {
        if(ChangeScene.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}
