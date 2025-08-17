using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2f; // auto-destroy after time
    public GameObject hitEffect; // optional particle effect

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player!");
            // damage player
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // Disable collider and renderer immediately to prevent further interactions
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        MeshRenderer rend = GetComponent<MeshRenderer>();
        if (rend != null) rend.enabled = false;

        // Destroy after short delay to allow other scripts to finish
        Destroy(gameObject, 0.05f);
    }

}