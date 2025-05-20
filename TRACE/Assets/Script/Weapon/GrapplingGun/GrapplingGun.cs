using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class GrapplingGun : MonoBehaviour {

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public bool isGrappled = false;
    public WeaponSwap ws;

    // Grapple Indicator
    public GameObject grappleIndicator; // A small red sphere to represent the grapple point
    public Image crosshair; // Canvas-based crosshair image

    private Vector3 currentGrapplePosition;

    void Awake() {
        lr = GetComponent<LineRenderer>();
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
        } else if (Input.GetMouseButtonUp(1)) {
            StopGrapple();
        }
    }

    void LateUpdate() {
        DrawRope();
    }

    void StartGrapple() {
        isGrappled = true;
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            isGrappled = true;
            grapplePoint = hit.point;

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;

            if (grappleIndicator != null) grappleIndicator.SetActive(false); // Hide indicator after grappling
        }
    }

    void StopGrapple() {
        isGrappled = false;
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope() {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.fixedDeltaTime * 8f);
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
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
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
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