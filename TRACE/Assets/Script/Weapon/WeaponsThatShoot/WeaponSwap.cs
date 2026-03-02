using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] weapons;
    
    public static int currentWeaponIndex = 0;
    
    [Header("References")]
    public GrapplingGun grapplingGun;
    public Pistol pistol;
    public Shotgun shotgun;
    

    void Start()
    {
        SelectWeapon(currentWeaponIndex);
    }

    void Update()
    {
        if (!grapplingGun.IsGrappling())
            HandleWeaponSwitchInput();
    }

    void HandleWeaponSwitchInput()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                SelectWeapon(i);
            }
        }
    }

    public void SelectWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;
        if (pistol.isReloading || shotgun.isReloading) return;
        
        PickUpItem pickUp = weapons[index].GetComponent<PickUpItem>();
        if (pickUp != null && !pickUp.PickedUp) return;

        currentWeaponIndex = index;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == currentWeaponIndex);
        }
    }
}