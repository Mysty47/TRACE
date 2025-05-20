using UnityEngine;

public class AmmoSelection : MonoBehaviour
{
    public int ammo1 = 12;
    public int ammo2 = 30;
    public WeaponScript ws;
    public WeaponSwap wsw;

    void Update()
    {
        if (wsw.currentWeaponIndex == 0)
        {
            ws.maxAmmo = ammo1;
        }
        else
        {
            ws.maxAmmo = ammo2;
        }
    }
}
