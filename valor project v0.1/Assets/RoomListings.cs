using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class RoomListings : MonoBehaviourPunCallbacks
{
    public GameObject roomListingPrefab;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject NewRoom = Instantiate(roomListingPrefab, GameObject.Find("Content").transform);
            NewRoom.GetComponent<Room>()._RoomName.text = roomInfo.Name;
            NewRoom.GetComponent<Room>()._playerCount.text = roomInfo.PlayerCount.ToString() + "/" + roomInfo.MaxPlayers.ToString();
        }
    }
}
