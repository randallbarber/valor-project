using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuUI : MonoBehaviourPunCallbacks
{
    // Main menu UI

    [SerializeField] Canvas SettingsMenu;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Slider slider;
    bool settingsOpen;
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenSettings()
    {
        if (settingsOpen == false)
        {
            settingsOpen = true;
            SettingsMenu.enabled = true;
        }
        else
        {
            settingsOpen = false;
            SettingsMenu.enabled = false;
        }
    }
    public void UpdateMouseSens(float NewSens)
    {
        MouseLook.MouseSens = NewSens;
        inputField.text = NewSens.ToString();
    }
    public void UpdateMouseSlider(string IFtext)
    {
        MouseLook.MouseSens = Mathf.Clamp(float.Parse(IFtext), 50, 750);
        slider.value = float.Parse(IFtext);
    }
    [SerializeField] string __________________________________________________________;
    // InGame UI

    [SerializeField] Canvas DeadScreen;
    [SerializeField] Canvas WinScreen;
    [SerializeField] Canvas InGameGUI;
    [SerializeField] Canvas PauseMenu;

    [SerializeField] Movement movementSC;
    [SerializeField] MouseLook MouseLookSC;
    bool MenuOpened = false;
    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");
    }
    public void GameEnded()
    {
            movementSC.enabled = false;
            MouseLookSC.enabled = false;
            InGameGUI.enabled = false;

            Cursor.lockState = CursorLockMode.Confined;
    }
    public void OpenMenu()
    {
        if (MenuOpened == false && DeadScreen.enabled == false && WinScreen.enabled == false)
        {
            MenuOpened = true;
            PauseMenu.enabled = true;
            Cursor.lockState = CursorLockMode.Confined;
            InGameGUI.enabled = false;
        }
        else if (DeadScreen.enabled == false && WinScreen.enabled == false)
        {
            MenuOpened = false;
            PauseMenu.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            InGameGUI.enabled = true;
        }
    }
}
