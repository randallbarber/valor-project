using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuUI : MonoBehaviourPunCallbacks
{
    [Header("Main Menu")]

    [SerializeField] Canvas SettingsMenu;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Slider slider;
    [SerializeField] Canvas __RoomList;
    [SerializeField] Canvas ModeSelection;
    [SerializeField] Canvas CreateSelection;
    bool settingsOpen;
    bool roomsOpen;
    bool modesOpen;
    bool createSelOpen;
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
    public void UpdateVolume(float NewVol)
    {
        AudioListener.volume = NewVol;
    }
    public void SeeRoomsList()
    {
        if (roomsOpen == false)
        {
            roomsOpen = true;
            __RoomList.enabled = true;
        }
        else
        {
            roomsOpen = false;
            __RoomList.enabled = false;
        }
    }
    public void SeeModes()
    {
        if (modesOpen == false)
        {
            modesOpen = true;
            ModeSelection.enabled = true;
        }
        else
        {
            modesOpen = false;
            ModeSelection.enabled = false;
        }
    }
    public void SeeCreateSettingsSelection()
    {
        if (createSelOpen == false)
        {
            createSelOpen = true;
            CreateSelection.enabled = true;
        }
        else
        {
            createSelOpen = false;
            CreateSelection.enabled = false;
        }
    }

    [Header("In Game")]

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
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MasterClient Has Left");
        }
    }
    public void PlayerKilled()
    {

        Cursor.lockState = CursorLockMode.Confined;
    }
    public void OpenMenu()
    {
        if (MenuOpened == false)
        {
            MenuOpened = true;
            PauseMenu.enabled = true;
            Cursor.lockState = CursorLockMode.Confined;
            InGameGUI.enabled = false;
        }
        else
        {
            MenuOpened = false;
            PauseMenu.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            InGameGUI.enabled = true;
        }
    }
}
