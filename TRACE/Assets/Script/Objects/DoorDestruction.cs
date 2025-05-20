using UnityEngine;

public class DoorDestruction : MonoBehaviour
{
    public GameObject Cover;
    public bool isGlass = false;
    public void DestroyWall()
    {
        // Loop through all child objects
        foreach (Transform piece in transform)
        {
            // Get the Rigidbody component of each child
            Rigidbody rb = piece.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                if (isGlass)
                {
                    rb.isKinematic = false;
                    Destroy(Cover, 0);
                    rb.AddExplosionForce(200f, transform.position, 5f); // Optional: Add an explosion effect
                }
                else
                {
                    rb.isKinematic = false; // Enable physics
                    Destroy(Cover, 0);
                    rb.AddExplosionForce(500f, transform.position, 5f); // Optional: Add an explosion effect
                }
            }
        }

        // Optional: Destroy the parent after a delay to clean up
        Destroy(gameObject, 1f);
    }
}