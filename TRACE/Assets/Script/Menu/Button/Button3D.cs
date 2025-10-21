using UnityEngine;

public class Button3D : MonoBehaviour
{
    public enum ButtonAction
    {
        RevealGameModes,
        Start,
        Endless,
        SelectLevel,
        Settings,
        Quit
    }
    [Header("References")]
    public ButtonAction action;
    public MenuController menuController;
    public MoveCameraForLevels moveCamera;
    public GameObject PlayText;
    public GameObject StartText;
    public GameObject EndlessText;

    public void OnClick()
    {
        if (menuController == null)
        {
            Debug.LogWarning("MenuController not assigned for: " + gameObject.name);
            return;
        }

        switch (action)
        {
            case ButtonAction.RevealGameModes:
                PlayText.SetActive(false);
                StartText.SetActive(true);
                EndlessText.SetActive(true);
                break;
            case ButtonAction.Start:
                menuController.StartLevel();
                break;
            case ButtonAction.Endless:
                menuController.StartEndless();
                break;
            case ButtonAction.SelectLevel:
                moveCamera.MoveCameraForLevelSection();
                break;
            case ButtonAction.Settings:
                moveCamera.MoveCameraForSettingsSection();
                break;
            case ButtonAction.Quit:
                menuController.Exit();
                break;
        }
    }
}