using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ApplySavedGraphics : MonoBehaviour
{
    private const string graphicsKey = "GraphicsQuality";
    public Camera cam;

    void Start()
    {
        int savedGraphics = PlayerPrefs.GetInt(graphicsKey, 1); // 1 = Good, 0 = Fast
        
        if (cam != null && cam.TryGetComponent<UniversalAdditionalCameraData>(out var urpCam))
        {
            urpCam.renderPostProcessing = (savedGraphics == 1);
        }
    }
}