using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class GroundedEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private Rigidbody rb;

    [Header("Settings")]
    public float sightRange = 15f;
    public float stopRange = 2f;
    public float heightAboveGround = 0.5f;
    public float recoverSpeed = 5f; // колко бързо се изправя след бутане

    [Header("Ground Check")]
    public LayerMask groundLayer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (!player)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj) player = playerObj.transform;
        }

        // NavMeshAgent настройки
        agent.updateUpAxis = false;
        agent.updatePosition = true;
    }

    void FixedUpdate()
    {
        if (!player) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 targetPos;

        if (distanceToPlayer <= stopRange)
        {
            targetPos = transform.position; // спира
        }
        else if (distanceToPlayer <= sightRange)
        {
            targetPos = player.position; // следва player
        }
        else
        {
            targetPos = transform.position; // стои на място (или можеш да добавиш random patrol)
        }

        agent.SetDestination(targetPos);

        // Регулиране на височината над земята
        Ray ray = new Ray(transform.position + Vector3.up * 1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, groundLayer))
        {
            Vector3 desiredPos = transform.position;
            desiredPos.y = Mathf.Lerp(transform.position.y, hit.point.y + heightAboveGround, Time.fixedDeltaTime * recoverSpeed);
            transform.position = desiredPos;
        }

        // Въртене към player
        if (distanceToPlayer <= sightRange && distanceToPlayer > stopRange)
        {
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.fixedDeltaTime * 5f);
        }
    }
}
