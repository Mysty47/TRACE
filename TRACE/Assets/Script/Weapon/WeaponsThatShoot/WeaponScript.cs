using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class WeaponScript : MonoBehaviour
{
    public float damageFromPlayerGun = 30f;
    public float range = 100f;
    public int maxAmmo;
    public bool isReloading = false;
    public int CurrentAmmo1;

    public GameObject trail;

    // public float fireRate = 0.1f; // Time between shots (e.g. 0.3s)
    // private float nextTimeToFire = 0f;
    public WeaponSwap ws;
    public GrapplingGun gg;
    public GunRecoil gr;
    private PlayerMovement pm;
    
    public Camera fpsCam;

    public ParticleSystem muzzleFlashPistol;
    
    public TextMeshProUGUI ammoText;
    
    public Animator animatorPistol;
    
    private Image AmmoIcon;
    
    [Header("Impact Effects")]
    
    public GameObject impactEffect;
    public GameObject enemyImpactEffectRedTriangles;
    public GameObject enemyImpactEffectWhiteTriangles;
    public GameObject impactEffectLongerVersion;
    
    [Header("AudioSource")]
    
    public AudioSource reloadSound;
    public AudioSource shootSound;
    void Start()
    {
        CurrentAmmo1 = 7;
        animatorPistol = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        AnimatorStateInfo state = animatorPistol.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("PistolReloadAnimation"))
        {
            trail.SetActive(true);
        }
        else trail.SetActive(false);

        if (ws.currentWeaponIndex == 0)
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

            if (!isReloading && CurrentAmmo1 > 0 && ws.currentWeaponIndex != 2 && !gg.swinging)
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
    if (gg.swinging == false)
    {
        // Handle ammo reduction and muzzle flash for current weapon
        if (ws.currentWeaponIndex == 0)
        {
            shootSound.Play();
            CurrentAmmo1 -= 1;
            if (muzzleFlashPistol != null) muzzleFlashPistol.Play();
            if (CurrentAmmo1 <= 0) StartCoroutine(Reload());
        }
        else if (ws.currentWeaponIndex == 1)
        {

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
                    
                    Target target = hit.transform.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamageTarget(damageFromPlayerGun);
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

    yield return null;

    animatorPistol.SetTrigger("ReloadAnimation");

    trail.SetActive(true);

    reloadSound.Play();

    AnimatorClipInfo[] clipInfo = animatorPistol.GetCurrentAnimatorClipInfo(0);

    float reloadTime = clipInfo.Length > 0 ? clipInfo[0].clip.length : 0.8f;

    yield return new WaitForSeconds(reloadTime);

    if (ws.currentWeaponIndex == 0)
    {
        CurrentAmmo1 = 7;
    }

    isReloading = false;
}




    private void ChangeTrigger(bool change)
    {
        animatorPistol.SetBool("ReloadAnimation", change);
    }
}
