using UnityEngine;

public class BladeAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public int damage = 25;
    public float attackRate = 1f;
    public LayerMask enemyLayer;
    public Animator animator;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Slash"); // Play attack animation

        // Check for enemies in range using a raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, enemyLayer))
        {
            Target enemy = hit.collider.GetComponent<Target>();
            if (enemy != null)
            {
                enemy.TakeDamageTarget(damage);
            }

            Debug.Log("Hit: " + hit.collider.name);
        }
    }
}