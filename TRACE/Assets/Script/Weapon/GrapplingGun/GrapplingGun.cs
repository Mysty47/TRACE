using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class GrapplingGun : MonoBehaviour {
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cameraPlayer, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public bool isGrappled = false;
    public WeaponSwap ws;

    // Grapple Indicator
    public GameObject grappleIndicator; // A small red sphere to represent the grapple point
    public Image crosshair; // Canvas-based crosshair image

    private Vector3 currentGrapplePosition;

    void Awake() {
        if (grappleIndicator != null) grappleIndicator.SetActive(false); // Hide indicator initially
    }

    void Update() {
        UpdateCrosshairAndIndicator();
        if (ws.currentWeaponIndex != 0)
        {
         StopGrapple();
         Destroy(joint);
        }
        if (Input.GetMouseButtonDown(1)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(1)) {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        isGrappled = true;
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.position, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
    }

    void StopGrapple() {
        isGrappled = false;
        Destroy(joint);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }

    /// <summary>
    /// Updates the crosshair visibility and grapple indicator.
    /// </summary>
    void UpdateCrosshairAndIndicator() {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.position, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable)) {
            if (crosshair != null) crosshair.enabled = false; // Hide crosshair
            if (grappleIndicator != null) {
                grappleIndicator.SetActive(true);
                grappleIndicator.transform.position = hit.point; // Move indicator to hit point
            }
        } else {
            if (crosshair != null) crosshair.enabled = true; // Show crosshair
            if (grappleIndicator != null) grappleIndicator.SetActive(false);
        }
    }
}