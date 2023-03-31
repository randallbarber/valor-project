using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerInfoDetails : MonoBehaviourPun
{
    public void SetPlayerName(string SetName)
    {
        PhotonNetwork.NickName = SetName;
    }
}
