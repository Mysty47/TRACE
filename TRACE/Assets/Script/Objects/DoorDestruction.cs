using UnityEngine;

public class DoorDestruction : MonoBehaviour
{
    [Header("References")]
    public GameObject Cover;
    public Transform player;
    
    [Header("Settings")]
    public float explosionForce = 5f;
    public float explosionRadius = 2f;
        
    [Header("Audio")]
    public AudioSource doorDestruction;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(Cover);
            doorDestruction.Play();

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            foreach (Transform child in transform)
            {
                if (child.gameObject != Cover)
                {
                    Rigidbody rb = child.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.isKinematic = false;

                        Vector3 direction = (child.position - player.position).normalized;
                        rb.AddForce(direction * explosionForce, ForceMode.Impulse);
                    }
                }
            }

            Destroy(gameObject, 3f);
        }
    }
}