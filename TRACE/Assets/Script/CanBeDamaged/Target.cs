using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    float health = 100f;
    public Animator animator; // Reference to the Animator
    private bool isDead = false;
    // public TextMeshProUGUI healthText;
    // public TextMeshProUGUI playerTriesText;
    public EnemyAi enemyScript;
    public Outline outlineScript;
    
    [Header("Player")]
    public GameObject player;
    public Image healthBar;
    

    private void Update()
    {
    }

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
                // TODO: Retry menu pop-up
            }
            Die();
        }
    }


    private void Die()
    {
        animator.enabled = false;
        enemyScript.enabled = false;
        outlineScript.enabled = false;

        Destroy(gameObject, 2f);
    }
}