using UnityEngine;
using UnityEngine.UI;

public class SoundPlay : MonoBehaviour
{
    public AudioSource audioSource; // Drag and drop the AudioSource here

    public void PlaySound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
