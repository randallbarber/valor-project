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
    bool map1;
    bool map2;
    bool _testMode;

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

    public void MatchMakeMap1()
    {
        map1 = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public void MatchMakeMap2()
    {
        map2 = true;
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
            if (map1)
            {
                PhotonNetwork.LoadLevel("Map1");
            }
            if (map2)
            {
                PhotonNetwork.LoadLevel("Map2");
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
