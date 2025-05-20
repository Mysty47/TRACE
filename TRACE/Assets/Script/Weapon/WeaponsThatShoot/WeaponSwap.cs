using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    // Array to hold weapon GameObjects
    public GameObject[] weapons;

    // Index of the currently selected weapon
    public int currentWeaponIndex = 0;
    public WeaponScript ws;

    void Start()
    {
        // Ensure only the selected weapon is active at the start
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

    void SelectWeapon(int index)
    {
        if (!ws.isReloading)
        {
            if (index < 0 || index >= weapons.Length) return;
            
            currentWeaponIndex = index;
            
            // Activate the selected weapon and deactivate others
            for (int i = 0; i < weapons.Length; i++) {
                weapons[i].SetActive(i == currentWeaponIndex);
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
