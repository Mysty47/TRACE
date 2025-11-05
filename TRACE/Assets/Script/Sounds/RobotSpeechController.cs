using UnityEngine;
public class RobotSpeechController : MonoBehaviour
{
    [Header("Settings")]
    public bool hasStarted = false;
    public bool finished = false;
    [SerializeField] private static int lever = 0;
    
    [Header("References")]
    public AudioSource robotSound;
    
    
    void Update()
    {
        if (EscapeMenuController.isPaused)
        {
            robotSound.Pause();
        }
        else if (!EscapeMenuController.isPaused && lever == 0)
        {
            robotSound.UnPause();
        }
        if (lever == 1)
        {
            robotSound.Stop();
            finished = true;
        }
        if (!hasStarted && robotSound.isPlaying)
        {
            hasStarted = true;
        }

        if (!robotSound.isPlaying && hasStarted)
        {
            finished = true;
            lever = 1;
        }
    }
}
