using UnityEngine;

public class ProceduralWalker : MonoBehaviour
{
    [Header("References")]
    public Transform body;
    public ProceduralWalker otherLeg;

    [Header("Step Settings")]
    public float footSpacing = 0.5f;
    public float stepDistance = 0.4f;
    public float stepHeight = 0.2f;
    public float stepSpeed = 4f;
    public LayerMask terrainLayer;

    private Vector3 oldPosition, newPosition, currentPosition;
    private Vector3 previousBodyPosition;
    private bool isMoving = false;
    private float stepProgress = 1f;

    void Start()
    {
        oldPosition = newPosition = currentPosition = transform.position;
        previousBodyPosition = body.position;
    }

    void Update()
    {
        transform.position = currentPosition;

        // body movement speed
        Vector3 bodyVelocity = (body.position - previousBodyPosition) / Time.deltaTime;
        previousBodyPosition = body.position;

        // position of the foot
        float forwardOffset = Mathf.Clamp(bodyVelocity.magnitude * 0.3f, 0.25f, 0.6f);
        Vector3 rayOrigin = body.position + (body.right * footSpacing) + (body.forward * forwardOffset);

        // Raycast to ground
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit info, 10f, terrainLayer))
        {
            if (!isMoving && (!otherLeg || !otherLeg.isMoving))
            {
                float distance = Vector3.Distance(newPosition, info.point);
                if (distance > stepDistance)
                {
                    newPosition = info.point;
                    stepProgress = 0f;
                    isMoving = true;
                }
            }
        }

        // leg movement
        if (isMoving)
        {
            stepProgress += Time.deltaTime * stepSpeed;

            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, stepProgress);
            footPosition.y += Mathf.Sin(stepProgress * Mathf.PI) * stepHeight;
            currentPosition = footPosition;

            if (stepProgress >= 1f)
            {
                isMoving = false;
                oldPosition = newPosition;
            }
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(newPosition, 0.05f);
    // }
}
