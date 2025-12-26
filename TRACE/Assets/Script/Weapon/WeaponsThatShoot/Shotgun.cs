using System.Collections;
using TMPro;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("References")] public Rigidbody rb;
    public PlayerMovement playerScript;
    public Camera fpsCam;
    public WeaponSwap ws;

    [Header("Crosshairs")] public UnityEngine.UI.Image ReloadCrosshair;
    public UnityEngine.UI.Image NormalCrosshair;

    [Header("Audio")] public AudioSource reloadSound;
    public AudioSource shootSound;

    [Header("Particles")] public ParticleSystem shootParticles;

    [Header("Animation")] public Animator anim;

    [Header("Tag")] private const string PadTag = "ShotgunPad";

    [Header("UI")] public TextMeshProUGUI ammoText;

    [Header("Weapon Settings")] 
    public float pushForce = 30f;
    public int maxAmmo = 5;
    public float range = 20f;
    public int currentAmmo;
    public float reloadTime = 2f;
    public bool readyToShoot = true;
    public bool isReloading = false;
    public int damage = 4;
    public float forceBoost = 3f;
    public float shootDelay = 0.5f;

    [Header("Constants")] private const string enemyTag = "Enemy";

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (isReloading) return;

        ammoText.text = currentAmmo.ToString();

        if (readyToShoot && Input.GetMouseButtonDown(0) && currentAmmo > 0 && WeaponSwap.currentWeaponIndex == 1 &&
            !isReloading)
        {
            Shoot();
            Invoke(nameof(ResetShotgunRotation), 1f);
        }

        else if ((currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R)) && WeaponSwap.currentWeaponIndex == 1 &&
                 currentAmmo != maxAmmo && !isReloading)
        {
            isReloading = true;

            if (reloadSound != null) reloadSound.Play();
            if (anim != null) anim.SetTrigger("Reload");

            Invoke(nameof(Reload), reloadTime);
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        if (shootSound != null) shootSound.Play();

        currentAmmo--;

        if (shootParticles != null) shootParticles.Play();

        if (anim != null) anim.SetTrigger("Shot");

        if (!playerScript.crouching)
        {
            float multiplier = 1f;
            
            RaycastHit hitPad;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitPad, range))
            {
                if (hitPad.transform.CompareTag(PadTag)) 
                {
                    multiplier = forceBoost;
                } else multiplier = 1f;
            }
            Vector3 knockbackDir = (-fpsCam.transform.forward + fpsCam.transform.up * 0.35f).normalized;
            rb.AddForce(knockbackDir * (pushForce * multiplier), ForceMode.Impulse);
        }

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

        Invoke(nameof(ResetAttack), shootDelay);
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

        if (anim != null) anim.ResetTrigger("Reload");

        ammoText.text = currentAmmo.ToString();
    }

    private void ResetShotgunRotation()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
