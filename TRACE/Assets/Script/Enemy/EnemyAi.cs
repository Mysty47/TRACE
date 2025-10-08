using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    public Target playerTargetScript;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;
    public Animator animator;
    
    [Header("Gravity")]
    public float downforce = 10f;
    
    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks = 1f;
    bool alreadyAttacked;
    public GameObject projectile;
    public float projectileSpeed = 32f; 
    public bool projectileUsesGravity = false;

    [Header("States")]
    public float sightRange = 15f;
    public float attackRange = 10f;
    public bool playerInSightRange, playerInAttackRange;
    
    [Header("Audio Settings")]
    public AudioSource walkSound;
    public AudioSource shootSound;
    
    

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool("Walking", true);
        walkSound.loop = true;
    }

    private void Update()
    {
        if (!player) return;

        // Detect player by distance only (tag-based)
        float distance = Vector3.Distance(transform.position, player.position);
        playerInSightRange = distance <= sightRange;
        playerInAttackRange = distance <= attackRange;

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patrolling()
    {
        animator.SetBool("Walking", true);
        
        if(!walkSound.isPlaying && walkSound != null) walkSound.Play();
        
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint + Vector3.up * 2, Vector3.down, out RaycastHit hit, 4f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        animator.SetBool("Walking", true);
        if(!walkSound.isPlaying && walkSound != null) walkSound.Play();
        if(walkPointSet)
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        // Smooth rotation toward player
        Vector3 lookDir = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        if (!alreadyAttacked)
        {
            animator.SetBool("Walking", false);
            ShootAtPlayer();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootAtPlayer()
    {
        if(walkSound.isPlaying && walkSound != null) walkSound.Stop();
        Vector3 spawnPos = transform.position + (player.position - transform.position).normalized * 1.5f + Vector3.up * 2.5f;
        Vector3 direction = (player.position - spawnPos).normalized;

        // Add bloom
        float bloomAmount = 0.2f; // tweak this for more or less inaccuracy
        direction.x += Random.Range(-bloomAmount, bloomAmount);
        direction.y += Random.Range(-bloomAmount, bloomAmount);
        direction.z += Random.Range(-bloomAmount, bloomAmount);
        direction.Normalize(); // normalize again to keep speed consistent

        GameObject bulletInstance = Instantiate(projectile, spawnPos, Quaternion.identity);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

        if (shootSound != null)
        {
            shootSound.Play();
        }
        
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = direction * projectileSpeed;
        }
    }



    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
