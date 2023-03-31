using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerCount : MonoBehaviourPunCallbacks
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] TMP_Text playerCountText;
    [SerializeField] Canvas canvasCount;
    [SerializeField] GameObject serverMSG;
    [SerializeField] Transform contentView;
    int playerCount;
    int playersReady;

    private void Start()
    {
        UpdatePlayerCount();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerCount();
        Debug.Log("playerjoined");
        GameObject srvrMSG = Instantiate(serverMSG, contentView);
        Debug.Log(srvrMSG.name);
        srvrMSG.GetComponent<TMP_Text>().text = newPlayer.NickName + " has joined the game";
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    private void UpdatePlayerCount()
    {
            playerCount = PhotonNetwork.PlayerList.Length;
            playerCountText.text = playersReady + "/" + playerCount;
    }

    public void PlayerReadyUp()
    {
        playersReady += 1;
        playerCountText.text = playersReady + "/" + playerCount;
        if (playersReady == playerCount)
        {
            canvasCount.enabled = false;
            enemySpawner.RoundBegin();
        }
    }
}
