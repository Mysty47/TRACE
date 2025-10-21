using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class Target : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public EnemyAi enemyScript;
    public Outline outlineScript;
    
    public ParticleSystem robotDeathParticles;
    public GameObject enemyGun;

    [Header("Settings")]
    public float health = 100f;
    public bool isDead = false;
    public bool isEnemyDead = false;

    [Header("Player")]
    public GameObject player;
    public Image healthBar;
    public GameObject retryCanvas;
    
    // Ragdoll data (само за врагове)
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Ако това е Enemy — подготвяме ragdoll логиката
        if (CompareTag("Enemy"))
        {
            ragdollBodies = GetComponentsInChildren<Rigidbody>();
            ragdollColliders = GetComponentsInChildren<Collider>();
            SetRagdollActive(false);
        }
    }

    void Update()
    {
        animator.SetBool("Dead", isEnemyDead);
    }

    public void TakeDamageTarget(float amount)
    {
        if (isDead) return;

        health -= amount;

        // Ако е Player — обновяваме health бара
        if (CompareTag("Player") && healthBar != null)
        {
            healthBar.fillAmount = health / 100f;
        }

        Debug.Log($"{gameObject.name} Health: {health}");

        if (health <= 0f)
        {
            isDead = true;

            if (CompareTag("Player"))
            {
                // Player умира → спира играта
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                if (retryCanvas != null) retryCanvas.SetActive(true);
            }

            Die();
        }
    }

    private void Die()
    {
        // Спиране на логиките
        isEnemyDead = true;
        if (animator != null) animator.enabled = false;
        if (outlineScript != null) outlineScript.enabled = false;
        if (enemyScript != null)
        {
            enemyScript.enabled = false;
            if (enemyScript.shootSound != null) enemyScript.shootSound.Stop();
            if (enemyScript.walkSound != null) enemyScript.walkSound.Stop();
        }

        // Само врагове ползват ragdoll
        if (CompareTag("Enemy"))
        {
            if (robotDeathParticles != null)
            {
                robotDeathParticles.Play();
            }
            enemyGun.SetActive(false);
            SetRagdollActive(true);

            // Много лека сила, за да се “отпусне” естествено
            Rigidbody hips = GetComponentInChildren<Rigidbody>();
            if (hips != null && player != null)
            {
                Vector3 dir = (transform.position - player.transform.position).normalized + Vector3.up * 0.3f;
                hips.AddForce(dir * 50f, ForceMode.Impulse); // не повече от 50–80
            }

            // Изтриване след 10 секунди
            Destroy(gameObject, 1f);
        }
    }

    private void SetRagdollActive(bool active)
    {
        if (ragdollBodies == null || ragdollColliders == null) return;

        // Изключваме CharacterController или Rigidbody на root-а
        var controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = !active;

        var mainRb = GetComponent<Rigidbody>();
        if (mainRb != null)
        {
            mainRb.isKinematic = true;
            mainRb.useGravity = false; // root-ът не трябва да участва
        }

        foreach (var rb in ragdollBodies)
        {
            if (rb != null)
            {
                rb.isKinematic = !active;
                rb.useGravity = active; // Включваме гравитацията само когато ragdoll е активен
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }

        foreach (var col in ragdollColliders)
        {
            if (col != null && col.gameObject != gameObject)
                col.enabled = active;
        }
    }
}
