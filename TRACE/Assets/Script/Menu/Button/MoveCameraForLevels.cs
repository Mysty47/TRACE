using UnityEngine;

public class MoveCameraForLevels : MonoBehaviour
{
    [Header("References")]
    public Transform camera;
    
    [Header("Settings")]
    public float rotationSpeed = 3f;
    private Quaternion targetRotation;
    private bool shouldRotate = false;

    void Update()
    {
        if (shouldRotate)
        {
            // Smooth transition
            camera.rotation = Quaternion.Slerp(camera.rotation, targetRotation,Time.deltaTime * rotationSpeed);
            
            if (Quaternion.Angle(camera.rotation, targetRotation) < 0.1f)
            {
                camera.rotation = targetRotation; 
                shouldRotate = false;
            }
        }
    }
    
    // Rotation
    public void MoveCameraForLevelSection()
    {
        targetRotation = Quaternion.Euler(0, 80, 0);
        shouldRotate = true;
    }

    public void MoveCameraForSettingsSection()
    {
        targetRotation = Quaternion.Euler(-80, 0, 0);
        shouldRotate = true;
    }

    // Reset
    public void ResetCameraPosition()
    {
        targetRotation = Quaternion.Euler(0, 0, 0);
        shouldRotate = true;
    }
}