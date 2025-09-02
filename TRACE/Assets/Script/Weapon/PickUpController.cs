using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public WeaponScript gunScript;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public Outline outline;

    public float pickUpRange;
    public float dropForwardForce, dropUpWardForce;

    public bool equipped;
    public static bool slotFull;

    void Start()
    {
       // Setup
       if (!equipped)
       {
           gunScript.enabled = false;
           coll.isTrigger = false;
       }

       if (equipped)
       {
           gunScript.enabled = true;
           coll.isTrigger = true;
           slotFull =  true;
       }
           
    }
    void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        outline.enabled = false;
        equipped = true;
        slotFull = true;
        
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        
        coll.isTrigger = true;
        
        gunScript.enabled = true;
    }
}
