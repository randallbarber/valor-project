using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MasterClientServices : MonoBehaviourPun
{
    private void Awake()
    {
        SetSceneForNewPlayers();
    }
    public void SetSceneForNewPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "scene", SceneManager.GetActiveScene().name } });
        }
    }
}
