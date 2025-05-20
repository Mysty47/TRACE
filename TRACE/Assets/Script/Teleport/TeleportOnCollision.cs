using UnityEngine;

public class TeleportOnCollision : MonoBehaviour
{
    public Vector3 teleportPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportPosition;
            Debug.Log("Player teleported to: " + teleportPosition);
        }
    }
}