using UnityEngine;
using Photon.Pun;
using TMPro;

public class DebugMenu : MonoBehaviourPun
{
    [SerializeField] Canvas Options;
    [SerializeField] GameObject serverMSG;
    [SerializeField] Transform contentView;
    bool OptionsOpen;
    public void OpenOptions()
    {
        if (OptionsOpen)
        {
            OptionsOpen = false;
            Options.enabled = false;
        }
        else
        {
            OptionsOpen = true;
            Options.enabled = true;
        }
    }
    private void Update()
    {
        if (OptionsOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    public void SendMSG()
    {
        photonView.RPC("SendServerMsg", RpcTarget.All, "DEBUG");
    }
    [PunRPC]
    void SendServerMsg(string MSG)
    {
        GameObject srvrMSG = Instantiate(serverMSG, contentView);
        srvrMSG.GetComponent<TMP_Text>().text = PhotonNetwork.NickName + " sent message: ''" + MSG + "'' to all clients";
    }
}
