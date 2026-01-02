using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase
{
    [Header("References")]
    public Image healthBar;
    public GameObject retryCanvas;
    
    [Header("Settings")]
    public bool isDead = false;

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (healthBar != null)
            healthBar.fillAmount = health / 100f;
    }

    protected override void Die()
    {
        isDead = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        if (retryCanvas != null)
            retryCanvas.SetActive(true);
    }
}