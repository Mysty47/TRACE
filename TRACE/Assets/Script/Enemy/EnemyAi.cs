using UnityEngine;
using UnityEngine.AI;
using UnityEditor;


public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    public Target playerTargetScript;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;
    public WeaponScript ws;
    public Outline outline;
    public Camera fpsCam;

    [Header("Shooting")]
    public Transform gunTip; // 👈 ново — позицията, от която ще се spawn-ва куршумът
    public GameObject projectile;
    public float projectileSpeed = 32f;
    public bool projectileUsesGravity = false;
    public float timeBetweenAttacks = 1f;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("States")]
    public float sightRange = 15f;
    public float attackRange = 10f;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Audio Settings")]
    public AudioSource walkSound;
    public AudioSource shootSound;

    private bool alreadyAttacked;
    [SerializeField] private bool isPlayerAimedAtYou;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        agent = GetComponent<NavMeshAgent>();
        outline =  GetComponent<Outline>();
        outline.enabled = false;
    }

    private void Start()
    {
        if (walkSound != null) walkSound.loop = true;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 100f))
        {
            if (hit.transform.gameObject == gameObject)
                isPlayerAimedAtYou = true;
            else
                isPlayerAimedAtYou = false;
        }
        else
        {
            isPlayerAimedAtYou = false;
        }

        if (isPlayerAimedAtYou)
            outline.enabled = true;
        else 
            outline.enabled = false;
        
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);
        playerInSightRange = distance <= sightRange;
        playerInAttackRange = distance <= attackRange;

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (walkSound != null && !walkSound.isPlaying) walkSound.Play();

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

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
        if (walkSound != null && !walkSound.isPlaying) walkSound.Play();
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
            ShootAtPlayer();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootAtPlayer()
    {
        if (walkSound != null && walkSound.isPlaying) walkSound.Stop();

        if (shootSound != null) shootSound.Play();

        // Ако имаме зададен shootPoint → използваме него
        Vector3 spawnPos;
        Quaternion spawnRot;

        if (gunTip != null)
        {
            spawnPos = gunTip.position;
            spawnRot = gunTip.rotation;
        }
        else
        {
            // fallback — ако не е зададен shootPoint
            spawnPos = transform.position + Vector3.up * 2f;
            spawnRot = transform.rotation;
        }

        // Изчисляваме посоката към играча
        Vector3 direction = (player.position - spawnPos).normalized;

        // Добавяме малък bloom за неточност
        float bloom = 0.2f;
        direction.x += Random.Range(-bloom, bloom);
        direction.y += Random.Range(-bloom, bloom);
        direction.z += Random.Range(-bloom, bloom);
        direction.Normalize();

        // Създаваме куршума
        GameObject bulletInstance = Instantiate(projectile, spawnPos, Quaternion.LookRotation(direction));
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = projectileUsesGravity;
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
