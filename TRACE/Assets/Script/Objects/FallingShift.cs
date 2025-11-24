using System.Collections;
using UnityEngine;

public class FallingShift : MonoBehaviour
{
    [Header("Settings")]
    public GameObject Holder;
    public Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.isKinematic = false;
            StartCoroutine(DisableAfterDelay(2f));
        }
    }
    
    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Holder.SetActive(false);
    }
}