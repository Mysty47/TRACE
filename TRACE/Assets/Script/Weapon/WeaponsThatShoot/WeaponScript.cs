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
    public int CurrentAmmo2;
    
    
    public WeaponSwap ws;
    public GrapplingGun gg;
    public GunRecoil gr;
    
    public GameObject glassForDestruction;
    public GameObject glassForDestruction1;
    public GameObject glassForDestruction2;
    public GameObject glassForDestruction3;
    public GameObject Cover;
    public GameObject Cover1;
    public GameObject Cover2;
    public GameObject Cover3;
    public Camera fpsCam;
    public ParticleSystem muzzleFlashSMG;
    public ParticleSystem muzzleFlashPistol;
    public GameObject impactEffect;
    public TextMeshProUGUI ammoText;
    public Animator animatorPistol;
    public Image AmmoIcon;

    void Start()
    {
        CurrentAmmo1 = 12;
        CurrentAmmo2 = 30;
        animatorPistol = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {

        if (ws.currentWeaponIndex == 0)
        {
            if (AmmoIcon != null && AmmoIcon.enabled == false)
            {
                AmmoIcon.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo1 != 12 && !isReloading) 
            {
                animatorPistol.SetTrigger("ReloadAnimation");
                StartCoroutine(Reload());
            }
            ammoText.text = CurrentAmmo1.ToString() + "/12";
        }

        if (ws.currentWeaponIndex == 1)
        {
            if (AmmoIcon != null && AmmoIcon.enabled == false)
            {
                AmmoIcon.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo2 != 30 && !isReloading) 
            {
                StartCoroutine(Reload());
            }
            ammoText.text = CurrentAmmo2.ToString() + "/30";
        }

        if (ws.currentWeaponIndex == 2)
        {
            range = 2f;
            ammoText.text = "";
            if (AmmoIcon != null && AmmoIcon.enabled == true)
            {
                AmmoIcon.enabled = false;
            }   
        }

        if (isReloading) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (!isReloading && (CurrentAmmo1 > 0 || CurrentAmmo2 > 0) && ws.currentWeaponIndex != 2 && !gg.isGrappled)
            {
                Shoot();
                gr.Recoil();
            }
            else if ((CurrentAmmo1 <= 0 || CurrentAmmo2 <= 0) && !isReloading && !gg.isGrappled)
            {
                animatorPistol.SetTrigger("ReloadAnimation");
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
            CurrentAmmo2 -= 1;
            if (muzzleFlashSMG != null) muzzleFlashSMG.Play();
            if (CurrentAmmo2 <= 0) StartCoroutine(Reload());
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            // Activate all glass pieces if "CoverForGlass" is hit
            if (hit.transform.name == "CoverForGlass")
            {
                if (glassForDestruction != null)
                {
                    // Enable the parent object to activate all child pieces
                    glassForDestruction.SetActive(true);
                    Cover.SetActive(false);
                    DoorDestruction glass = glassForDestruction.GetComponent<DoorDestruction>();
                    glass.isGlass = true;
                    if (glass != null)
                    {
                        glass.DestroyWall();
                    }
                }
            }
            else if (hit.transform.name == "CoverForGlass1")
            {
                if (glassForDestruction1 != null)
                {
                    // Enable the parent object to activate all child pieces
                    glassForDestruction1.SetActive(true);
                    Cover1.SetActive(false);
                    DoorDestruction glass = glassForDestruction1.GetComponent<DoorDestruction>();
                    glass.isGlass = true;
                    if (glass != null)
                    {
                        glass.DestroyWall();
                    }
                }
            }
            else if (hit.transform.name == "CoverForGlass2")
            {
                if (glassForDestruction2 != null)
                {
                    // Enable the parent object to activate all child pieces
                    glassForDestruction2.SetActive(true);
                    Cover2.SetActive(false);
                    DoorDestruction glass = glassForDestruction2.GetComponent<DoorDestruction>();
                    glass.isGlass = true;
                    if (glass != null)
                    {
                        glass.DestroyWall();
                    }
                }
            }
            else if (hit.transform.name == "CoverForGlass3")
            {
                if (glassForDestruction3 != null)
                {
                    // Enable the parent object to activate all child pieces
                    glassForDestruction3.SetActive(true);
                    Cover3.SetActive(false);
                    DoorDestruction glass = glassForDestruction3.GetComponent<DoorDestruction>();
                    glass.isGlass = true;
                    if (glass != null)
                    {
                        glass.DestroyWall();
                    }
                }
            }
            else if (hit.transform.name == "BreakingWall")
            {
                DoorDestruction wall = hit.transform.GetComponent<DoorDestruction>();
                if (wall != null) wall.DestroyWall();
            }
            else
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
}



    private IEnumerator Reload()
    {
        if (isReloading) yield break; // Prevent multiple reloads at once

        isReloading = true;

        // Wait for the reload animation to finish
        float reloadTime = 0.8f; // Match this to your reload animation duration
        yield return new WaitForSeconds(reloadTime);

        // Refill ammo after the animation finishes
        if (ws.currentWeaponIndex == 0) // Pistol
        {
            CurrentAmmo1 = 12;
        }
        else if (ws.currentWeaponIndex == 1) // SMG
        {
            CurrentAmmo2 = 30;
        }

        isReloading = false;
    }



    private void ChangeTrigger(bool change)
    {
        animatorPistol.SetBool("ReloadAnimation", change);
    }
}
