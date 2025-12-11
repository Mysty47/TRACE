using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] weapons;
    public static int currentWeaponIndex = 0;
    
    [Header("References")]
    public WeaponScript ws;
    public Shotgun shotgun;
    public WeaponScript weaponScript;

    void Start()
    {
        SelectWeapon(currentWeaponIndex);
    }

    void Update()
    {
        HandleWeaponSwitchInput();
    }

    void HandleWeaponSwitchInput()
    {
        // Switch weapons using number keys
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
        if (weaponScript != null)
        {
            if (weaponScript.isReloading || shotgun.isReloading) return;
            if (index < 0 || index >= weapons.Length) return;

            // check which weapons are picked
            PickUpItem pickUp = weapons[index].GetComponent<PickUpItem>();
            if (pickUp != null && !pickUp.PickedUp)
            {
                return;
            }

            currentWeaponIndex = index;

            for (int i = 0; i < weapons.Length; i++)
            {
                PickUpItem p = weapons[i].GetComponent<PickUpItem>();
                if (p != null && p.PickedUp)
                {
                    weapons[i].SetActive(i == currentWeaponIndex);
                }
            }
        }
    }


    void SelectNextWeapon()
    {
        int nextWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
        SelectWeapon(nextWeaponIndex);
    }

    void SelectPreviousWeapon()
    {
        int previousWeaponIndex = (currentWeaponIndex - 1 + weapons.Length) % weapons.Length;
        SelectWeapon(previousWeaponIndex);
    }
}
