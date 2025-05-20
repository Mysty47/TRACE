using System.Collections;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Transform gunTransform;      // The gun's transform
    public Transform muzzlePoint;       // The muzzle point (gun tip)
    public Vector3 gunRecoilOffset = new Vector3(0, 0, -0.05f); // Small backward gun movement
    public float gunRecoilSpeed = 10f;  // Speed of gun position recovery
    public float muzzleRecoilAngle = -10f; // How far the muzzle rotates upward (X rotation)
    public float muzzleReturnSpeed = 20f; // Speed of muzzle returning to original rotation

    private Vector3 gunOriginalPosition;
    private Quaternion gunOriginalRotation;
    private Quaternion muzzleOriginalRotation;

    void Start()
    {
        // Store the original positions and rotations
        if (gunTransform == null) gunTransform = transform;
        if (muzzlePoint == null)
        {
            Debug.LogError("Muzzle Point not assigned. Assign a Transform for the muzzle point.");
            return;
        }

        gunOriginalPosition = gunTransform.localPosition;
        gunOriginalRotation = gunTransform.localRotation;
        muzzleOriginalRotation = muzzlePoint.localRotation;
    }

    public void Recoil()
    {
        // Reset the gun and muzzle to their original state before applying new recoil
        gunTransform.localPosition = gunOriginalPosition;
        gunTransform.localRotation = gunOriginalRotation;
        muzzlePoint.localRotation = muzzleOriginalRotation;

        // Start both recoil effects
        StopAllCoroutines();
        StartCoroutine(ApplyGunRecoil());
        StartCoroutine(ApplyMuzzleRecoil());
    }

    private IEnumerator ApplyGunRecoil()
    {
        // Slightly move the gun backward
        Vector3 targetPosition = gunOriginalPosition + gunRecoilOffset;
        while (Vector3.Distance(gunTransform.localPosition, targetPosition) > 0.01f)
        {
            gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, targetPosition, Time.deltaTime * gunRecoilSpeed);
            yield return null;
        }

        // Return the gun to its original position
        while (Vector3.Distance(gunTransform.localPosition, gunOriginalPosition) > 0.01f)
        {
            gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, gunOriginalPosition, Time.deltaTime * gunRecoilSpeed);
            yield return null;
        }

        gunTransform.localPosition = gunOriginalPosition;
    }

    private IEnumerator ApplyMuzzleRecoil()
    {
        // Rotate the muzzle upward by adjusting its local X rotation
        Quaternion targetRotation = muzzleOriginalRotation * Quaternion.Euler(muzzleRecoilAngle, 0, 0);
        while (Quaternion.Angle(muzzlePoint.localRotation, targetRotation) > 0.1f)
        {
            muzzlePoint.localRotation = Quaternion.Lerp(muzzlePoint.localRotation, targetRotation, Time.deltaTime * muzzleReturnSpeed);
            yield return null;
        }

        // Return the muzzle to its original rotation
        while (Quaternion.Angle(muzzlePoint.localRotation, muzzleOriginalRotation) > 0.1f)
        {
            muzzlePoint.localRotation = Quaternion.Lerp(muzzlePoint.localRotation, muzzleOriginalRotation, Time.deltaTime * muzzleReturnSpeed);
            yield return null;
        }

        muzzlePoint.localRotation = muzzleOriginalRotation;
    }
}
