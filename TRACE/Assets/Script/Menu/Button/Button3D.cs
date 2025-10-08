using UnityEngine;

public class Button3D : MonoBehaviour
{
    public enum ButtonAction
    {
        Start,
        SelectLevel,
        Settings,
        Quit
    }
    [Header("References")]
    public ButtonAction action;
    public MenuController menuController;
    public MoveCameraForLevels moveCamera;

    public void OnClick()
    {
        if (menuController == null)
        {
            Debug.LogWarning("MenuController not assigned for: " + gameObject.name);
            return;
        }

        switch (action)
        {
            case ButtonAction.Start:
                menuController.StartLevel();
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