using UnityEngine;
using System.Collections;

public class Pistol : WeaponBase
{
    [Header("Pistol Stats")]
    public float pistolDamage = 1f;
    public float pistolRange = 100f;
    public float pistolFireDelay = 0.25f;
    public int pistolMaxAmmo = 7;
    public float pistolReloadTime = 0.8f;
    
    [Header("References")]
    public GunRecoil recoil;
    public GrapplingGun gg;

    [Header("Visuals")]
    public ParticleSystem muzzleFlash;
    public Animator animator;

    [Header("Audio")]
    public AudioSource shootSound;
    public AudioSource reloadSound;
    
    protected override void ApplyWeaponStats()
    {
        damage = pistolDamage;
        range = pistolRange;
        shootDelay = pistolFireDelay;
        maxAmmo = pistolMaxAmmo;
    }

    protected override void Shoot()
    {
        if (!gg.swinging)
        {
            base.Shoot();
            
            shootSound?.Play();
            recoil?.Recoil();
            muzzleFlash?.Play();
        }
    }

    protected override IEnumerator Reload()
    {
        animator.SetTrigger("ReloadAnimation");
        reloadSound?.Play();
        if(!gg.swinging)
            return base.Reload();
        else return null;
    }

    protected override float GetReloadTime()
    {
        return pistolReloadTime;
    }
}