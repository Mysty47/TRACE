using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource backGroundMusic;
    public Slider backGroundMusicSlider;
    private const string volumeKey = "MusicVolume";

    [Header("Graphics")]
    public Camera mainCamera;
    public Button fastButton;
    public Button goodButton;
    private const string graphicsKey = "GraphicsQuality";

    [Header("Sensitivity")]
    public Slider sensitivitySlider;
    private const string sensitivityKey = "MouseSensitivity";

    void Start()
    {
        // --- AUDIO ---
        float savedVolume = PlayerPrefs.GetFloat(volumeKey, 0.75f);
        backGroundMusicSlider.value = savedVolume;
        SetVolume(savedVolume);
        backGroundMusicSlider.onValueChanged.AddListener(SetVolume);

        // --- GRAPHICS ---
        int savedGraphics = PlayerPrefs.GetInt(graphicsKey, 1);
        ApplyGraphicsSettings(savedGraphics == 1);

        if (fastButton != null)
            fastButton.onClick.AddListener(SetFastGraphics);

        if (goodButton != null)
            goodButton.onClick.AddListener(SetGoodGraphics);

        // --- SENSITIVITY ---
        float savedSensitivity = PlayerPrefs.GetFloat(sensitivityKey, 100f);
        sensitivitySlider.value = savedSensitivity;
        SetSensitivity(savedSensitivity);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    // --- AUDIO ---
    public void SetVolume(float volume)
    {
        if (backGroundMusic != null)
            backGroundMusic.volume = volume;

        PlayerPrefs.SetFloat(volumeKey, volume);
    }

    // --- GRAPHICS ---
    public void SetFastGraphics()
    {
        ApplyGraphicsSettings(false);
        PlayerPrefs.SetInt(graphicsKey, 0);
        PlayerPrefs.Save();
    }

    public void SetGoodGraphics()
    {
        ApplyGraphicsSettings(true);
        PlayerPrefs.SetInt(graphicsKey, 1);
        PlayerPrefs.Save();
    }

    private void ApplyGraphicsSettings(bool isGood)
    {
        if (mainCamera != null && mainCamera.TryGetComponent(out UniversalAdditionalCameraData urpCam))
        {
            urpCam.renderPostProcessing = isGood;
        }
    }

    // --- SENSITIVITY ---
    public void SetSensitivity(float sens)
    {
        PlayerMovement.sensitivity = sens;
        PlayerPrefs.SetFloat(sensitivityKey, sens);
        PlayerPrefs.Save();
    }
}
