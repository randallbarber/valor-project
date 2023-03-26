using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] Canvas canvas;
    [SerializeField] Canvas ErrorCanvas;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] TMP_Text ErrorTextLabel;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        ErrorCanvas.enabled = false;
        textLabel.enabled = false;
        canvas.enabled = true;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        ErrorCanvas.enabled = true;
        ErrorTextLabel.text = "Error: Failed to connect to server";
    }
    public void TryReConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}
