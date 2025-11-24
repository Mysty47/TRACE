using UnityEngine;
using UnityEditor;


public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    public ProceduralWalker leftLeg;
    public ProceduralWalker rightLeg;
    public Transform player;

    [Header("Ranges")]
    public float detectRange = 5f;
    public float stopRange = 2f;
    public float randomWalkRadius = 3f;

    [Header("Step Settings")]
    public float randomMoveCooldown = 2f;

    private Vector3 randomTarget;
    private float timer;

    void Start()
    {
        randomTarget = GetRandomPosition();
        timer = randomMoveCooldown;
    }

    void Update()
    {
        if (!player) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        Vector3 targetPosition;

        // Stopping
        if (distanceToPlayer <= stopRange)
        {
            targetPosition = transform.position;
        }
        // Following Player
        else if (distanceToPlayer <= detectRange)
        {
            targetPosition = player.position;
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0f || Vector3.Distance(transform.position, randomTarget) < 0.2f)
            {
                randomTarget = GetRandomPosition();
                timer = randomMoveCooldown;
            }
            targetPosition = randomTarget;
        }

        // Targets for both legs
        if (leftLeg) leftLeg.transform.position = targetPosition;
        if (rightLeg) rightLeg.transform.position = targetPosition;

        // rotation to the player if in distance
        if (distanceToPlayer <= detectRange && distanceToPlayer > stopRange)
        {
            Vector3 lookDir = (player.position - transform.position);
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * randomWalkRadius;
        Vector3 randomPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        
        if (Physics.Raycast(randomPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
        {
            randomPos.y = hit.point.y;
        }

        return randomPos;
    }
}
