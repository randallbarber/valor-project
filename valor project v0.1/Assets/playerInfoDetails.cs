using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerInfoDetails : MonoBehaviourPun
{
    private void Start()
    {
        string randomNumber = Random.Range(1000, 2000).ToString();
        PhotonNetwork.NickName = "Player" + randomNumber;
    }
    public void SetPlayerName(string SetName)
    {
        PhotonNetwork.NickName = SetName;
    }
}
