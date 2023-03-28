using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{
    public TMP_Text _RoomName;
    public TMP_Text _playerCount;
    public void JoinRoom()
    {
        GameObject.Find("CreateAndJoinRooms").GetComponent<CreateAndJoinRooms>().JoinRoomOnList(_RoomName.text);
    }

}
