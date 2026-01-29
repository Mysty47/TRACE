using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpItem : MonoBehaviour
{
    [Header("References")]
    public WeaponBase ws;
    public WeaponSwap weaponSwap;
    private GrapplingGun gg;
    private Collider coll;
    private Outline outline;
    public PickUpController puc;
    public InstructionsController ic;
    public WeaponSelection weapon;
    
    [Header("Settings")]
    public bool PickedUp = false;
    public float instructionsTimeDelay = 3f;
    public Vector3 pistolLocalScale = new Vector3(0.04f, 0.04f, 0.04f);
    public Quaternion pistolLocalRotation =  Quaternion.Euler(0f, 90f, 0f);
    public Vector3 shotgunLocalScale = new Vector3(13f, 13f, 13f);
    public Quaternion shotgunLocalRotation = Quaternion.Euler(0f, 0f, 0f);
    
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
                coll.enabled = true;
        }
        else
        {
            if(ws != null)
                ws.enabled = true;
            if(gg != null)
                gg.enabled = true;
            if(coll != null)
                coll.enabled = false;
            
            if (outline != null) outline.enabled = false;
        }
    }

    public void OnPickUp(Transform parent)
    {
        PickedUp = true;
        ws.enabled = true;
        if (outline != null) Destroy(outline);
        ManageInstructions();
        
        transform.SetParent(parent, false);
        transform.localPosition = Vector3.zero;
        
        if (weapon == WeaponSelection.Pistol)
        {
            transform.localRotation = pistolLocalRotation;
            transform.localScale = pistolLocalScale;
        }
        else if(weapon == WeaponSelection.Shotgun)
        {
            transform.localRotation = shotgunLocalRotation;
            transform.localScale = shotgunLocalScale;
            if(ws != null) SwapToTheWeaponOnPickup(1);
        }
        
        coll.isTrigger = true;
        ws.enabled = true;
        gg.enabled = true;
        
        if (outline != null)
            outline.enabled = false;
        
        puc.enabled = false;
    }

    private void ManageInstructions()
    {
        ic.ShowText();
        Invoke(nameof(HideInstructionText), instructionsTimeDelay);
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