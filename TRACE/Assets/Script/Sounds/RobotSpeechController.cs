using UnityEngine;
public class RobotSpeechController : MonoBehaviour
{
    public bool hasStarted = false;
    public bool called = false;
    public AudioSource robotSound;
    void Update()
    {
        if (!hasStarted && robotSound.isPlaying)
        {
            hasStarted = true;
        }
        if (!robotSound.isPlaying && hasStarted) called = true;
    }
}
