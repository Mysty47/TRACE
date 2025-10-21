using UnityEngine;
public class RobotSpeechController : MonoBehaviour
{
    public bool hasStarted = false;
    public bool finished = false;
    public AudioSource robotSound;
    [SerializeField] private static int lever = 0;
    void Update()
    {
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
