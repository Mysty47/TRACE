using UnityEngine;

public class AmmoSelection : MonoBehaviour
{
    [Header("Settings")]
    public int ammo1 = 12;
    public int ammo2 = 30;
    
    [Header("References")]
    public WeaponScript ws;
    public WeaponSwap wsw;

    void Update()
    {
        if (WeaponSwap.currentWeaponIndex == 0)
        {
            ws.maxAmmo = ammo1;
        }
        else
        {
            ws.maxAmmo = ammo2;
        }
    }
}
