using UnityEngine;

public class OpenningElevator : MonoBehaviour
{
    [Header("References")]
    public Transform movingObject;
    public Transform startPoint;
    public Transform endPoint;
    private PickUpItem currentItem;

    [Header("Settings")]
    public float speed = 2f;
    public bool isItFromPickingWeapon = false;
    private bool isMoving = false;
    

    void Start()
    {
        if (isItFromPickingWeapon)
        {
            currentItem = GetComponent<PickUpItem>();
        }
        if (movingObject != null && startPoint != null) 
            movingObject.position = startPoint.position;
    }

    void Update()
    {
        if (isMoving && movingObject != null)
        {
            // Move smoothly towards endPoint
            Moving();
        }
        else if (isItFromPickingWeapon && currentItem.PickedUp)
        {
            Moving();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

    public void Moving()
    {
        movingObject.position = Vector3.MoveTowards(
            movingObject.position,
            endPoint.position,
            speed * Time.deltaTime
        );
    }
}