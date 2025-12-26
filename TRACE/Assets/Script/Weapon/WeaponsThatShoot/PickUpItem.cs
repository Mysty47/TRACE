using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpItem : MonoBehaviour
{
    [Header("References")]
    public WeaponScript ws;
    public WeaponSwap weaponSwap;
    private GrapplingGun gg;
    private Collider coll;
    private Outline outline;
    public PickUpController puc;
    public InstructionsController ic;
    
    [Header("Settings")]
    public bool PickedUp = false;
    public WeaponSelection weapon;
    
    public enum WeaponSelection
    {
        Pistol, 
        Shotgun
    }

    void Awake()
    {
        if (!PickedUp)
        {
            coll = GetComponent<Collider>();
            outline = GetComponent<Outline>();
            gg = GetComponent<GrapplingGun>();
            if (outline != null) outline.enabled = false;
            if(ws != null)
                ws.enabled = false;
            if(gg != null)
                gg.enabled = false;
            if(coll != null)
                coll.isTrigger = false;
        }
        else
        {
            if(ws != null)
                ws.enabled = true;
            if(gg != null)
                gg.enabled = true;
            if(coll != null)
                coll.isTrigger = false;
            
            if (outline != null) outline.enabled = false;
        }
    }

    public void OnPickUp(Transform parent)
    {
        PickedUp = true;
        if (outline != null) Destroy(outline);
        ManageInstructions();
        
        transform.SetParent(parent, false);
        transform.localPosition = Vector3.zero;
        
        if (weapon == WeaponSelection.Pistol)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(0.03f,0.03f, 0.03f);
        }
        else if(weapon == WeaponSelection.Shotgun)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = Vector3.one;
            SwapToTheWeaponOnPickup(1);
        }
        
        coll.isTrigger = true;
        ws.enabled = true;
        gg.enabled = true;

        // GunRecoil recoil = GetComponent<GunRecoil>();
        // if (recoil != null)
        // {
        //     recoil.ResetRecoilOrigin();
        // }
        
        if (outline != null)
            outline.enabled = false;
        
        puc.enabled = false;
    }

    private void ManageInstructions()
    {
        ic.ShowText();
        Invoke(nameof(HideInstructionText), 3f);
    }
    
    private void HideInstructionText()
    {
        ic.HideText();
    }

    private void SwapToTheWeaponOnPickup(int index)
    {
        weaponSwap.weapons[index] = gameObject;
        WeaponSwap.currentWeaponIndex = index;
        weaponSwap.SelectWeapon(index);
    }
}