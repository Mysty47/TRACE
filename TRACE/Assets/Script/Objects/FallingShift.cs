using System.Collections;
using UnityEngine;

public class FallingShift : MonoBehaviour
{
    public GameObject Holder;
    public Rigidbody rb;
    // This is called when the player collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check the name or tag of the collided object
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.isKinematic = false;
            StartCoroutine(DisableAfterDelay(2f));
        }
    }
    
    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Holder.SetActive(false); // disable the GameObject
    }
}