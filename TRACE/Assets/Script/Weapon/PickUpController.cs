using System;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform gunContainer;
    public Transform shotgunContainer;
    public Camera fpsCam;
    public WeaponSwap ws;

    [Header("Settings")]
    public float pickUpRange = 3f;
    public LayerMask pickableLayer;
    private Outline currentOutline;
    private PickUpItem currentItem;

    private bool slotFull = false;
    
    void Update()
    {
        HandleRaycast();
    }

    void HandleRaycast()
    {
        RaycastHit hit;
        if (Physics.SphereCast(fpsCam.transform.position, 0.3f, fpsCam.transform.forward, out hit, pickUpRange, pickableLayer))
        {
            PickUpItem item = hit.transform.GetComponent<PickUpItem>();

            if (item != null)
            {
                if (currentItem != item)
                {
                    ClearOutline();
                    currentItem = item;
                    currentOutline = item.GetComponent<Outline>();
                    if (currentOutline != null)
                        currentOutline.enabled = true;
                }
                
                if (Input.GetKeyDown(KeyCode.E) && !slotFull)
                {
                    PickUp(item);
                }
            }
        }
        else
        {
            ClearOutline();
        }
    }

    void ClearOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
        currentItem = null;
    }

    void PickUp(PickUpItem item)
    {
        slotFull = true;
        switch (item.weapon)
        {
            case PickUpItem.WeaponSelection.Pistol:
               item.OnPickUp(gunContainer);
               break;
            case PickUpItem.WeaponSelection.Shotgun:
                item.OnPickUp(shotgunContainer);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        ClearOutline();
    }

    public void DropCurrent()
    {
        slotFull = false;
    }
}
