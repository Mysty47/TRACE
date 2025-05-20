using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.02f; // Количество на движението
    public float swaySpeed = 2.0f;   // Скорост на движението
    public float movementMultiplier = 1.5f; // Увеличение при движение

    private Vector3 initialPosition;

    void Start()
    {
        // Запазване на началната позиция на оръжието
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Вземи движенията на мишката
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        // Изчисляване на отклонението
        Vector3 swayOffset = new Vector3(-mouseX, -mouseY, 0);

        // Увеличение при движение (например при натискане на W)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            swayOffset *= movementMultiplier;
        }

        // Плавно движение към новата позиция
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + swayOffset, Time.deltaTime * swaySpeed);
    }
}