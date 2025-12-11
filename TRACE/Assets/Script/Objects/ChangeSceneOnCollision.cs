using Script.Texts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnCollision : MonoBehaviour
{
    [Header("Settings")]
    public int scene;
    public string objectName = "Player";
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == objectName)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
