using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Settings")]
    public float swayAmount = 0.02f;
    public float swaySpeed = 2.0f;
    public float movementMultiplier = 1.5f;

    private Vector3 initialPosition;

    void Start()
    {
        // saving original position of the weapon
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // input
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;
        
        Vector3 swayOffset = new Vector3(-mouseX, -mouseY, 0);
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            swayOffset *= movementMultiplier;
        }

        // smooth transition to the new position
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + swayOffset, Time.deltaTime * swaySpeed);
    }
}