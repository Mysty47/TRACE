using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public PlayerMovement playerScript;
    public Camera fpsCam;
    public WeaponSwap ws;
    public GameObject orientation;

    [Header("Crosshairs")]
    public UnityEngine.UI.Image ReloadCrosshair;
    public UnityEngine.UI.Image NormalCrosshair;

    [Header("Audio")]
    public AudioSource reloadSound;
    public AudioSource reloadFinishSound;
    public AudioSource shootSound;

    [Header("Weapon Settings")]
    public float pushForce = 15f;
    public int maxAmmo = 5;
    public float range = 20f;
    public float currentAmmo;
    public float reloadTime = 1.2f; 
    public bool readyToShoot = true;
    public bool isReloading = false;
    public int damage = 4;

    [Header("Constants")] 
    private const string enemyTag = "Enemy";

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (isReloading) return;

        if (readyToShoot && Input.GetMouseButtonDown(0) && currentAmmo > 0 && WeaponSwap.currentWeaponIndex == 1 && !isReloading)
        {
            Shoot();
        }

        else if ((currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R)) && WeaponSwap.currentWeaponIndex == 1)
        {
            isReloading = true;
            
            if(reloadSound != null) reloadSound.Play();
            
            Invoke(nameof(Reload), reloadTime);
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        if (shootSound != null) shootSound.Play();

        currentAmmo--;
        
        rb.AddForce();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.transform.CompareTag(enemyTag))
            {
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                    enemyHealth.TakeDamage(damage);
            }
        }

        Invoke(nameof(ResetAttack), 0.85f);
    }

    private void ResetAttack()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        // NormalCrosshair.gameObject.SetActive(false);
        // ReloadCrosshair.fillAmount = 0f;
        // ReloadCrosshair.gameObject.SetActive(true);
            
        // if (reloadSound != null) reloadSound.Play();
            
        // float elapsed = 0f;
            
        // while (elapsed < reloadTime)
        // {
        //     elapsed += Time.deltaTime;
        //
        //     ReloadCrosshair.fillAmount = Mathf.Clamp01(elapsed / reloadTime);
        //     if (elapsed >= reloadTime * 0.8f)
        //     {
        //         if (!reloadFinishSound.isPlaying) reloadFinishSound.Play();
        //     }
        // }
            
        currentAmmo = maxAmmo;
            
        // ReloadCrosshair.gameObject.SetActive(false);
        // NormalCrosshair.gameObject.SetActive(true);
            
        isReloading = false;
            
        readyToShoot = true;
        
    }
}
