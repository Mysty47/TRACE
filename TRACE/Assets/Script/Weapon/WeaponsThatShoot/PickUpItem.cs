using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpItem : MonoBehaviour
{
    [Header("References")]
    public WeaponScript ws;
    private GrapplingGun gg;
    private Collider coll;
    private Outline outline;
    public PickUpController puc;
    public InstructionsController ic;
    
    [Header("Settings")]
    public bool PickedUp = false;

    void Awake()
    {
        coll = GetComponent<Collider>();
        outline = GetComponent<Outline>();
        gg = GetComponent<GrapplingGun>();
        if (outline != null)
            outline.enabled = false;

        ws.enabled = false;
        gg.enabled = false;
        coll.isTrigger = false;
    }

    public void OnPickUp(Transform parent)
    {
        PickedUp = true;
        ManageInstructions();
        transform.SetParent(parent, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 90, 0);
        transform.localScale = new Vector3(0.03f,0.03f, 0.03f);

        coll.isTrigger = true;
        ws.enabled = true;
        gg.enabled = true;

        GunRecoil recoil = GetComponent<GunRecoil>();
        if (recoil != null)
        {
            recoil.ResetRecoilOrigin();
        }
        
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
}