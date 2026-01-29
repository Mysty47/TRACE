using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Runtime Stats")]
    protected float damage;
    protected float range;
    protected float shootDelay;
    protected int maxAmmo;

    protected int currentAmmo;
    protected bool readyToShoot = true;
    public bool isReloading;
    public bool areThereEnemiesInScene;

    [Header("References")]
    public Camera fpsCam;

    [Header("Impact Effects")]
    public GameObject impactEffect;
    public GameObject impactEffectLongerVersion;
    public GameObject whiteEffect;
    public GameObject redEffect;
    
    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public Image ReloadCrosshair;
    public Image NormalCrosshair;

    protected virtual void Start()
    {
        ApplyWeaponStats();
        currentAmmo = maxAmmo;
    }

    protected virtual void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isReloading) return;
        if(ammoText != null)
            ammoText.text = currentAmmo.ToString();
        
        if(currentAmmo == 0)
            StartCoroutine(Reload());

        if (Input.GetMouseButtonDown(0) && readyToShoot)
        {
            if (currentAmmo > 0 && !EscapeMenuController.isPaused)
                Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !EscapeMenuController.isPaused)
        {
            StartCoroutine(Reload());
        }
    }

    protected virtual void Shoot()
    {
        readyToShoot = false;
        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            SpawnImpact(hit);
            DealDamage(hit);
        }

        Invoke(nameof(ResetShoot), shootDelay);
    }

    protected virtual IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;

        NormalCrosshair.gameObject.SetActive(false);
        ReloadCrosshair.fillAmount = 0f;
        ReloadCrosshair.gameObject.SetActive(true);

        float reloadTime = GetReloadTime();
        float elapsed = 0f;

        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            ReloadCrosshair.fillAmount = elapsed / reloadTime;
            yield return null;
        }

        ReloadCrosshair.gameObject.SetActive(false);
        NormalCrosshair.gameObject.SetActive(true);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    protected virtual void DealDamage(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Enemy"))
        {
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if(areThereEnemiesInScene) SpawnEnemyImpact(hit);
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }

    protected virtual void SpawnImpact(RaycastHit hit)
    {
        if (impactEffect != null)
            Destroy(Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)), 2f);

        if (impactEffectLongerVersion != null)
            Destroy(Instantiate(impactEffectLongerVersion, hit.point, Quaternion.LookRotation(hit.normal)), 2f);
    }
    
    protected virtual void SpawnEnemyImpact(RaycastHit hit)
    {
        if (whiteEffect != null)
            Destroy(Instantiate(whiteEffect, hit.point, Quaternion.LookRotation(hit.normal)), 2f);

        if (redEffect != null)
            Destroy(Instantiate(redEffect, hit.point, Quaternion.LookRotation(hit.normal)), 2f);
    }

    protected void ResetShoot()
    {
        readyToShoot = true;
    }
    protected abstract void ApplyWeaponStats();
    protected abstract float GetReloadTime();
}
