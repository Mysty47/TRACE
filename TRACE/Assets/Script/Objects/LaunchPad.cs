using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    // Adjust this to control the launch force
    public float launchForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object has a Rigidbody
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Apply an upward force
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }
    }
}