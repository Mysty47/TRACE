using UnityEngine;
using UnityEditor;


public class CameraTiltAndBob : MonoBehaviour
{
    [Header("References")]
    public Rigidbody playerRb;
    public bool isGrounded = true;

    [Header("Tilt Settings")]
    public float tiltAmount = 4f;
    public float tiltSpeed = 6f;
    private float currentTilt = 0f;

    [Header("Headbob Settings")]
    public float bobSpeed = 6f;
    public float bobAmount = 0.05f;
    private float bobTimer = 0f;
    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float targetTilt = -horizontal * tiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        Vector3 velocity = playerRb.linearVelocity;
        Vector3 newLocalPos = startLocalPos;

        if (isGrounded && velocity.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            newLocalPos.y = startLocalPos.y + Mathf.Sin(bobTimer) * bobAmount;
        }
        else
        {
            bobTimer = 0f;
        }

        transform.localPosition = newLocalPos;

        transform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }
}