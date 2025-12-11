using Script.Texts;
using UnityEngine;

public class TeleportOnCollision : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 teleportPosition;
    
    [Header("Constants")]
    private const string PlayerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            other.transform.position = teleportPosition;
        }
    }
}