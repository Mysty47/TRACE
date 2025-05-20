using UnityEngine;

public class FallingShift : MonoBehaviour
{
    public GameObject Holder;
    // This is called when the player collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check the name or tag of the collided object
        if (collision.gameObject.CompareTag("Player"))
        {
            Holder.SetActive(false);
        }
    }
}