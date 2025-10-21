using UnityEngine;
using UnityEditor;


public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    public ProceduralWalker leftLeg;
    public ProceduralWalker rightLeg;
    public Transform player;

    [Header("Ranges")]
    public float detectRange = 5f;  // range за да започне да следва
    public float stopRange = 2f;    // range за да спре до player
    public float randomWalkRadius = 3f; // когато няма player

    [Header("Step Settings")]
    public float randomMoveCooldown = 2f; // колко често да сменя random позиция

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

        // Решаваме target позицията
        Vector3 targetPosition;

        if (distanceToPlayer <= stopRange)
        {
            // Спира на място
            targetPosition = transform.position;
        }
        else if (distanceToPlayer <= detectRange)
        {
            // Следва player
            targetPosition = player.position;
        }
        else
        {
            // Random движение
            timer -= Time.deltaTime;
            if (timer <= 0f || Vector3.Distance(transform.position, randomTarget) < 0.2f)
            {
                randomTarget = GetRandomPosition();
                timer = randomMoveCooldown;
            }
            targetPosition = randomTarget;
        }

        // Задаваме target за двата крака
        if (leftLeg) leftLeg.transform.position = targetPosition;
        if (rightLeg) rightLeg.transform.position = targetPosition;

        // Въртене към player, ако е в detect range
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

        // Optional: Raycast, за да стъпи на земята
        if (Physics.Raycast(randomPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
        {
            randomPos.y = hit.point.y;
        }

        return randomPos;
    }
}
