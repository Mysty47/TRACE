using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBase : MonoBehaviour
{
    [Header("Health Settings")]
    public float health = 100f;
    protected bool isDead = false;

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;
        
        health -= amount;
        Debug.Log($"{gameObject.name} Health: {health}");

        if (health <= 0f)
        {
            isDead = true;
            Die();
        }
    }

    protected abstract void Die();
}