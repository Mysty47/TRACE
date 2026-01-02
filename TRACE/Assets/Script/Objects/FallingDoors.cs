using UnityEngine;

public class FallingDoors : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 0.1f;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public bool soundPlayed = false;
    
    [Header("References")]
    public AudioSource openningDoorSound;
    public RobotSpeechController rsc;

    void Start()
    {
        transform.position = startPoint;
    }

    void Update()
    {
        if (!rsc.hasStarted && rsc.robotSound.isPlaying)
        {
            rsc.hasStarted = true;
        }
        if (!rsc.robotSound.isPlaying && rsc.hasStarted) rsc.finished = true;
        if (rsc.finished)
        {
            if (openningDoorSound != null && !soundPlayed)
            {
                openningDoorSound.Play();
                soundPlayed = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.fixedDeltaTime);
        }

    }
}
