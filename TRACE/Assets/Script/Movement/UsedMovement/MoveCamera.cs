using UnityEngine;

public class MoveCamera : MonoBehaviour 
{
    [Header("References")]
    public Transform player;

    void Update() {
        transform.position = player.transform.position;
    }
}