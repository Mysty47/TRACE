using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class Shotgun : WeaponBase
{
    [Header("References")]
    public Rigidbody rb;
    public PlayerMovement playerScript;
    public Animator anim;

    [Header("Shotgun Stats")]
    public float shotgunDamage = 4f;
    public float shotgunRange = 20f;
    public float shotgunFireDelay = 0.5f;
    public int shotgunMaxAmmo = 5;
    public float shotgunReloadTime = 2f;

    [Header("Knockback")]
    public float pushForce = 30f;
    public float forceBoost = 3f;

    [Header("Particles")]
    public ParticleSystem shootParticles;

    [Header("Audio")]
    public AudioSource shootSound;
    public AudioSource reloadSound;

    [Header("Tags")]
    private const string EnemyTag = "Enemy";
    private const string PadTag = "ShotgunPad";

    protected override void ApplyWeaponStats()
    {
        damage = shotgunDamage;
        range = shotgunRange;
        shootDelay = shotgunFireDelay;
        maxAmmo = shotgunMaxAmmo;
    }

    void OnEnable()
    {
        ResetShotgunRotation();
    }

    protected override void Shoot()
    {
        base.Shoot();

        shootSound?.Play();
        shootParticles?.Play();
        anim?.SetTrigger("Shot");
        
        ApplyKnockback();
    }

    protected override IEnumerator Reload()
    {
        anim?.SetTrigger("Reload");
        reloadSound?.Play();
        return base.Reload();
    }

    protected override float GetReloadTime()
    {
        return shotgunReloadTime;
    }

    protected override void DealDamage(RaycastHit hit)
    {
        if (hit.transform.CompareTag(EnemyTag))
        {
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damage);
        }
    }

    private void ApplyKnockback()
    {
        if (playerScript.crouching) return;

        float multiplier = 1f;
        RaycastHit hitPad;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitPad, range))
        {
            if (hitPad.transform.CompareTag(PadTag))
                multiplier = forceBoost;
        }

        Vector3 knockbackDir =
            (-fpsCam.transform.forward + fpsCam.transform.up * 0.35f).normalized;

        rb.AddForce(knockbackDir * (pushForce * multiplier), ForceMode.Impulse);

        Invoke(nameof(ResetShotgunRotation), 1f);
    }

    private void ResetShotgunRotation()
    {
        transform.localRotation = Quaternion.Euler(-90f, 0f, -90f);
    }
}
