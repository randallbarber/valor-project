using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerCount : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject serverMSG;
    [SerializeField] Transform contentView;

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        GameObject srvrMSG = Instantiate(serverMSG, contentView);
        srvrMSG.GetComponent<TMP_Text>().text = newPlayer.NickName + " has joined the game";
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player newPlayer)
    {
        GameObject srvrMSG = Instantiate(serverMSG, contentView);
        srvrMSG.GetComponent<TMP_Text>().text = newPlayer.NickName + " has left the game";
    }
}
