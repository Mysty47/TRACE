using UnityEngine;

public class EnemyHealth : HealthBase
{
    public Animator animator;
    public EnemyAi enemyScript;
    public Outline outlineScript;
    public ParticleSystem robotDeathParticles;
    public GameObject enemyGun;
    public GameObject player;

    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        SetRagdollActive(false);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    protected override void Die()
    {
        if (animator) animator.enabled = false;
        if (outlineScript) outlineScript.enabled = false;
        if (enemyScript)
        {
            enemyScript.enabled = false;
            enemyScript?.shootSound?.Stop();
            enemyScript?.walkSound?.Stop();
        }

        robotDeathParticles?.Play();
        if (enemyGun) enemyGun.SetActive(false);

        SetRagdollActive(true);

        // Apply light force
        Rigidbody hips = GetComponentInChildren<Rigidbody>();
        if (hips && player)
        {
            Vector3 dir = (transform.position - player.transform.position).normalized + Vector3.up * 0.3f;
            hips.AddForce(dir * 50f, ForceMode.Impulse);
        }

        Destroy(gameObject, 1f);
    }


    private void SetRagdollActive(bool active)
    {
        var controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = !active;

        var mainRb = GetComponent<Rigidbody>();
        if (mainRb != null)
        {
            mainRb.isKinematic = true;
            mainRb.useGravity = false;
        }

        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !active;
            rb.useGravity = active;
        }

        foreach (var col in ragdollColliders)
        {
            if (col.gameObject != gameObject)
                col.enabled = active;
        }
    }
}
