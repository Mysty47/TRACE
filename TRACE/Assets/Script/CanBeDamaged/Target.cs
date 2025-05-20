using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    [SerializeField] float health = 100f;
    private Animator animator; // Reference to the Animator
    private bool isDead = false;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI playerTriesText;

    private void Start()
    {
        // Get the Animator component from the GameObject
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject.");
        }
    }

    // private void Update()
    // {
    // }

    public void TakeDamageTarget(float amount)
    {
        if (isDead) return; // Prevent taking damage after death

        health -= amount;
        Debug.Log($"Current Health: {health}");
        

        if (health <= 0f)
        {
            isDead = true;
            Die();
        }
    }


    private void Die()
    {
        if (animator != null)
        {
            animator.SetBool("isDead", true); // Trigger the death animation
        }   
        else
        {
            Debug.LogError("Animator component not assigned.");
        }

        // Destroy the object after the animation plays
        Destroy(gameObject, 1f); // Adjust delay if needed
    }
}