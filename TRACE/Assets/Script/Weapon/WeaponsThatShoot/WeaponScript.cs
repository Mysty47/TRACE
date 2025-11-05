using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class WeaponScript : MonoBehaviour
{
    public bool areYouAimedAtRobot = false;
    public float damageFromPlayerGun = 30f;
    public float range = 100f;
    public int maxAmmo;
    public bool isReloading = false;
    public int CurrentAmmo1;

    public GameObject trail;

    // public float fireRate = 0.1f; // Time between shots (e.g. 0.3s)
    // private float nextTimeToFire = 0f;
    [Header("References")]
    public WeaponSwap ws;
    public GrapplingGun gg;
    public GunRecoil gr;
    private PlayerMovement pm;
    
    public Camera fpsCam;

    public ParticleSystem muzzleFlashPistol;
    
    public TextMeshProUGUI ammoText;

    [Header("Muzzle Flash Light")]
    public Light muzzleFlashLight;

    [Header("Images")]
    public Image ReloadCrosshair;
    public Image NormalCrosshair;
    private Image AmmoIcon;
    
    public Animator animatorPistol;
    
    [Header("Impact Effects")]
    
    public GameObject impactEffect;
    public GameObject enemyImpactEffectRedTriangles;
    public GameObject enemyImpactEffectWhiteTriangles;
    public GameObject impactEffectLongerVersion;
    
    [Header("AudioSource")]
    
    public AudioSource reloadSound;
    public AudioSource shootSound;
    public AudioSource reloadFinishSound;
    void Start()
    {
        CurrentAmmo1 = 7;
        animatorPistol = GetComponentInChildren<Animator>();
        if (muzzleFlashLight != null)
            muzzleFlashLight.enabled = false;
    }
    
    void Update()
    {
        AnimatorStateInfo state = animatorPistol.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("PistolReloadAnimation"))
        {
            trail.SetActive(true);
        }
        else trail.SetActive(false);

        if (ws.currentWeaponIndex == 0 && !EscapeMenuController.isPaused)
        {
            if (AmmoIcon != null && AmmoIcon.enabled == false)
            {
                AmmoIcon.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo1 != 7 && !isReloading)
            {
                StartCoroutine(Reload());
            }
            ammoText.text = CurrentAmmo1.ToString();
        }

        if (ws.currentWeaponIndex == 1)
        {

        }

        if (ws.currentWeaponIndex == 2)
        {
            
        }

        if (isReloading) return;

        if (Input.GetMouseButtonDown(0))
        {
            // nextTimeToFire = Time.time + fireRate;

            if (!isReloading && CurrentAmmo1 > 0 && ws.currentWeaponIndex != 2 && !gg.swinging && !EscapeMenuController.isPaused)
            {
                Shoot();
                if(gr != null) gr.Recoil();
            }
            else if ((CurrentAmmo1 <= 0) && !isReloading && !gg.swinging)
            {
                StartCoroutine(Reload());
            }
        }

    }

    void Shoot()
    {
    if (!gg.swinging)
    {
        if (muzzleFlashLight != null)
        {
            StartCoroutine(MuzzleFlashLightEffect());
        }
        // Handle ammo reduction and muzzle flash for current weapon
        if (ws.currentWeaponIndex == 0)
        {
            shootSound.Play();
            CurrentAmmo1 -= 1;
            if (muzzleFlashPistol != null) muzzleFlashPistol.Play();
            if (CurrentAmmo1 <= 0) StartCoroutine(Reload());
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    if (enemyImpactEffectRedTriangles != null)
                    {
                        GameObject impact = Instantiate(enemyImpactEffectRedTriangles, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(impact, 2f);
                    }
                    
                    if (enemyImpactEffectWhiteTriangles != null)
                    {
                        GameObject impact = Instantiate(enemyImpactEffectWhiteTriangles, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(impact, 2f);
                    }
                    
                    EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damageFromPlayerGun);
                    }
                }

                if (impactEffect == null) return;
                {
                    GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 2f);
                }
                
                if (impactEffectLongerVersion == null) return;
                {
                    GameObject impact = Instantiate(impactEffectLongerVersion, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 2f);
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;

        // 🔹 Скриваме нормалния crosshair, показваме кръглия
        NormalCrosshair.gameObject.SetActive(false);
        ReloadCrosshair.fillAmount = 0f;
        ReloadCrosshair.gameObject.SetActive(true);

        animatorPistol.SetTrigger("ReloadAnimation");
        trail.SetActive(true);

        float reloadTime = 0.8f;
        float elapsed = 0f;

        reloadSound.Play();

        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            ReloadCrosshair.fillAmount = Mathf.Clamp01(elapsed / reloadTime);

            if (elapsed >= reloadTime - 0.3f && !reloadFinishSound.isPlaying)
            {
                reloadFinishSound.Play();
            }

            yield return null;
        }

        ReloadCrosshair.fillAmount = 1f;
        yield return new WaitForSeconds(0.1f);

        ReloadCrosshair.gameObject.SetActive(false);
        NormalCrosshair.gameObject.SetActive(true);

        CurrentAmmo1 = 7;
        isReloading = false;
    }
    private void ChangeTrigger(bool change)
    {
        animatorPistol.SetBool("ReloadAnimation", change);
    }
    
    private IEnumerator MuzzleFlashLightEffect()
    {
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(0.2f);
        muzzleFlashLight.enabled = false;
    }

}
