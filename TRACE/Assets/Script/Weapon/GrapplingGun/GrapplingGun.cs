using UnityEngine;
using UnityEngine.UI; // Required for UI components
using EZCameraShake;

public class GrapplingGun : MonoBehaviour {
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cameraPlayer, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public bool isGrappled = false;
    public float AimAssistSize = 1f;
    public WeaponSwap ws;

    // Grapple Indicator
    public GameObject grappleIndicator; // A small red sphere to represent the grapple point
    public Image crosshair; // Canvas-based crosshair image

    private Vector3 currentGrapplePosition;

    [Header("Audio Source")]
    public AudioSource grapplingGunSound;

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
        RaycastHit hit;
        if (Physics.SphereCast(cameraPlayer.position, AimAssistSize ,cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable))
        {
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
            grapplingGunSound.Play();
            isGrappled = true;
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
        // Use SphereCast with AimAssistSize radius like in StartGrapple
        if (Physics.SphereCast(cameraPlayer.position, AimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable)) {
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