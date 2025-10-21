using UnityEngine;

public class CameraTiltAndBob : MonoBehaviour
{
    [Header("References")]
    public Rigidbody playerRb; // Player Rigidbody
    public PlayerMovement playerMovement; // твоят PlayerMovement

    [Header("Tilt Settings")]
    public float tiltAmount = 10f; // колко се накланя камерата
    public float tiltSpeed = 8f;   // колко бързо се накланя

    [Header("Headbob Settings")]
    public float bobFrequency = 6f;    // честота на боба
    public float bobAmplitude = 0.05f; // колко силен да е
    public float bobSmoothing = 8f;    // изглаждане

    private Vector3 initialLocalPos;
    private float bobTimer;
    private float currentTilt;

    void Start()
    {
        // запомняме началната позиция на камерата спрямо holder-а
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        HandleTilt();
        HandleHeadbob();
    }

    void HandleTilt()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // target tilt (въртим само около Z оста)
        float targetTilt = -horizontalInput * tiltAmount;

        // плавно стигаме до target
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        // прилагаме само въртене по Z, без да пипаме останалите ротации
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, currentTilt);
    }

    void HandleHeadbob()
    {
        // ако не сме на земята — няма headbob
        if (!playerMovement.grounded)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * bobSmoothing);
            return;
        }

        Vector3 velocity = playerRb.linearVelocity;
        Vector3 flatVel = new Vector3(velocity.x, 0, velocity.z);

        if (flatVel.magnitude > 0.1f)
        {
            // движение => bob effect
            bobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            Vector3 targetPos = initialLocalPos + new Vector3(0f, bobOffset, 0f);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * bobSmoothing);
        }
        else
        {
            // стоим на място => плавно се връща
            bobTimer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * bobSmoothing);
        }
    }
}
