using UnityEngine;

public class GrapplingBow : MonoBehaviour
{
    [Header("References")] 
    private BetaPlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    
    [Header("Grappling")] 
    public float maxGrappleDistance;
    public float grappleDelayTime;
    
    private Vector3 grapplePoint;

    [Header("Cooldown")] 
    public float grapplingCd;
    private float grapplingCdTimer;
    
    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse0;
    
    private bool  grappling;
    
    
    void Start()
    {
        pm.GetComponent<BetaPlayerMovement>();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(grappleKey)) StartGrapple();
        
        if(grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;
        
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {
        
    }

    private void StopGrapple()
    {
        grappling = false;
        
        grapplingCdTimer = grapplingCd;
    }
}
