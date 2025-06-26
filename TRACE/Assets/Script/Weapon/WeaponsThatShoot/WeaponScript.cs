using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public PlayerMovement pm;
    public GameObject impactEffect;
    
    public Camera fpsCam;

    public ParticleSystem muzzleFlashPistol;
    
    public TextMeshProUGUI ammoText;
    
    public Animator animatorPistol;
    
    private Image AmmoIcon;

    void Start()
    {
        CurrentAmmo1 = 12;
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
            if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo1 != 12 && !isReloading)
            {
                StartCoroutine(Reload());
            }
            ammoText.text = CurrentAmmo1.ToString();
            Debug.Log(CurrentAmmo1);
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

            if (!isReloading && CurrentAmmo1 > 0 && ws.currentWeaponIndex != 2 && !gg.isGrappled)
            {
                Shoot();
                if(gr != null) gr.Recoil();
            }
            else if ((CurrentAmmo1 <= 0) && !isReloading && !gg.isGrappled)
            {
                StartCoroutine(Reload());
            }
        }

    }

    void Shoot()
    {
    if (gg.isGrappled == false)
    {
        // Handle ammo reduction and muzzle flash for current weapon
        if (ws.currentWeaponIndex == 0)
        {
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
                // Handle other targets
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamageTarget(damageFromPlayerGun);
                }
            }

            // Spawn impact effect
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
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

    AnimatorClipInfo[] clipInfo = animatorPistol.GetCurrentAnimatorClipInfo(0);

    float reloadTime = clipInfo.Length > 0 ? clipInfo[0].clip.length : 0.8f;

    yield return new WaitForSeconds(reloadTime);

    if (ws.currentWeaponIndex == 0)
    {
        CurrentAmmo1 = 12;
    }

    isReloading = false;
}




    private void ChangeTrigger(bool change)
    {
        animatorPistol.SetBool("ReloadAnimation", change);
    }
}
