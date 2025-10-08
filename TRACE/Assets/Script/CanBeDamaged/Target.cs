using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public EnemyAi enemyScript;
    public Outline outlineScript;
    public GameObject retryCanvas;
    
    [Header("Settings")]
    float health = 100f;
    private bool isDead = false;
    
    [Header("Player")]
    public GameObject player;
    public Image healthBar;

    public void TakeDamageTarget(float amount)
    {
        if (isDead) return;
        
        health -= amount;
        
        if (gameObject.name == "Player")
        {
            healthBar.fillAmount = health / 100f;
        }
        
        Debug.Log($"Current Health: {health}");
        

        if (health <= 0f)
        {
            isDead = true;
            if (gameObject.name == "Player")
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                retryCanvas.SetActive(true);
            }
            Die();
        }
    }


    private void Die()
    {
        animator.enabled = false;
        enemyScript.enabled = false;
        outlineScript.enabled = false;
        enemyScript.shootSound.Stop();
        enemyScript.walkSound.Stop();

        Destroy(gameObject, 2f);
    }
}