using UnityEngine;
using UnityEngine.UI; // Required for UI components
using EZCameraShake;

public class GrapplingGun : MonoBehaviour {
    [Header("Settings")]
    private Vector3 swingPoint;
    private float maxDistance = 100f;
    public bool swinging = false;
    public float AimAssistSize = 1f;
    
    [Header("References")]
    public WeaponSwap ws;
    private SpringJoint joint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cameraPlayer, player;
    
    [Header("UI")]
    public GameObject grappleIndicator; // A small red sphere to represent the grapple point
    public Image crosshair; // Canvas-based crosshair image
    public Image dashBar;

    private Vector3 currentGrapplePosition;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse1;
    
    
    [Header("Audio Source")]
    public AudioSource swingGunSound;

    void Awake() {
        if (grappleIndicator != null) grappleIndicator.SetActive(false); // Hide indicator initially
    }

    void Update() {
        UpdateCrosshairAndIndicator();
        if (ws.currentWeaponIndex != 0)
        {
         StopSwing();
         Destroy(joint);
        }
        if (Input.GetKeyDown(swingKey)) {
            StartSwing();
        }
        else if (Input.GetKeyUp(swingKey)) {
            StopSwing();
        }
    }

    void StartSwing()
    {
        RaycastHit hit;
        if (Physics.SphereCast(cameraPlayer.position, AimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable))
        {
            // Prevention of grappling through walls
            if (!Physics.Linecast(cameraPlayer.position, hit.point, LayerMask.GetMask( "Ground", "Climbable"))) 
            {
                CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
                swingGunSound.Play();
                swinging = true;
                swingPoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = swingPoint;

                float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

                // The distance grapple will try to keep from grapple point
                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;

                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;
            }
        }
    }


    void StopSwing() {
        swinging = false;
        
        Destroy(joint);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return swingPoint;
    }
    
    // Updates the crosshair visibility and grapple indicator
    void UpdateCrosshairAndIndicator() {
        RaycastHit hit;
        // Use SphereCast with AimAssistSize radius like in StartGrapple
        if (Physics.SphereCast(cameraPlayer.position, AimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable)) {
            if (crosshair != null) crosshair.enabled = false; // Hide crosshair
            if (dashBar != null) dashBar.enabled = false; // Hide dashBar
            
            if (grappleIndicator != null) {
                grappleIndicator.SetActive(true);
                grappleIndicator.transform.position = hit.point; // Move indicator to hit point
            }
        } else {
            if (crosshair != null) crosshair.enabled = true; // Show crosshair
            if (dashBar != null) dashBar.enabled = true;
            if (grappleIndicator != null) grappleIndicator.SetActive(false);
        }
    }

}