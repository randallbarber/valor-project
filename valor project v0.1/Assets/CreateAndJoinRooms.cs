using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput;
    [SerializeField] TMP_InputField joinInput;
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    public void MatchMake()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public void JoinRoomOnList(string _RoomName)
    {
        PhotonNetwork.JoinRoom(_RoomName);
    }    
}
