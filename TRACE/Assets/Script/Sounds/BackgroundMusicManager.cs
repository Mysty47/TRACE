using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("References")]
    private static BackgroundMusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
}
