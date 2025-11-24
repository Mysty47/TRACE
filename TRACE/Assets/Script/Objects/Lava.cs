using UnityEngine;

public class Lava : MonoBehaviour
{
    [Header("References")]
    public GameObject RetryCanvas;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            RetryCanvas.SetActive(true);
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.isDead = true;
        }
    }
}