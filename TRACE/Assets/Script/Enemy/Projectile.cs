using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2f; // auto-destroy after time
    public GameObject hitEffect; // optional particle effect

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player!");
            // TODO: damage player here
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // Destroy immediately (or short delay if you want particles to play)
        Destroy(gameObject);
    }
}