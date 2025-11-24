using Script.Texts;
using UnityEngine;

public class TeleportOnCollision : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 teleportPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportPosition;
        }
    }
}