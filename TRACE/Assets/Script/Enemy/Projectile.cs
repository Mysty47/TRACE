using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2f; // auto-destroy after time
    public GameObject hitEffect; // optional particle effect

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit");
            Target target =  other.GetComponent<Target>();
            OnHit();
            target.TakeDamageTarget(50);
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
    
    // Decrease Transparency
    public void OnHit()
    {
        StopAllCoroutines();
        StartCoroutine(ResetAlpha());
    }
    
    private System.Collections.IEnumerator ResetAlpha()
    {
        yield return new WaitForSeconds(0.1f);
    }
}