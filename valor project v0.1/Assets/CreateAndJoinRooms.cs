using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput;
    [SerializeField] TMP_InputField joinInput;
    bool pvpMode;
    bool _testMode;

    // ZOMBIE MODE//
    public void MatchMake()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    public void JoinRoomOnList(string _RoomName)
    {
        PhotonNetwork.JoinRoom(_RoomName);
    }

    // PVP //
    public void MatchMakePVP()
    {
        pvpMode = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    // TEST // 
    public void TestMode()
    {
        _testMode = true;
        PhotonNetwork.CreateRoom("test");
    }

    // JOIN ROOM WITH CORRECT SETTINGS/MODE //
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (pvpMode)
            {
                PhotonNetwork.LoadLevel("Map1");
            }
            if (_testMode)
            {
                PhotonNetwork.LoadLevel("TestMap");
            }
        }
        else
        {
            string sceneName = (string)PhotonNetwork.CurrentRoom.CustomProperties["scene"];
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
}
