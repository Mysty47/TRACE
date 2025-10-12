using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Menu.Sound
{
    public class GlobalMusicManager : MonoBehaviour
    {
        public static GlobalMusicManager Instance { get; private set; }
        public AudioSource music;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene.ToLower().Contains("mainmenu"))
            {
                if (!music.isPlaying)
                    music.Play();
                return;
            }
            
            if (EscapeMenuController.isPaused)
            {
                if (music.isPlaying)
                    music.Pause();
            }
            else
            {
                if (!music.isPlaying)
                    music.Play();
            }
        }
    }
}