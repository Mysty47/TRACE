using UnityEngine;

public class OpenningElevator : MonoBehaviour
{
    [Header("References")]
    public Transform movingObject;
    public Transform startPoint;
    public Transform endPoint;

    [Header("Settings")]
    public float speed = 2f;

    private bool isMoving = false;

    void Start()
    {
        if (movingObject != null && startPoint != null)
            movingObject.position = startPoint.position;
    }

    void Update()
    {
        if (isMoving && movingObject != null)
        {
            // Move smoothly towards endPoint
            movingObject.position = Vector3.MoveTowards(
                movingObject.position,
                endPoint.position,
                speed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }
}