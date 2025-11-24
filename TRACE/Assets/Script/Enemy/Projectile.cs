using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    public GameObject hitEffect;
    public PlayerHealth playerHealth;
    
    [Header("Settings")]
    public float lifeTime = 2f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit");
            HealthBase healthBase =  other.GetComponent<HealthBase>();
            OnHit();
            playerHealth.TakeDamage(50);
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