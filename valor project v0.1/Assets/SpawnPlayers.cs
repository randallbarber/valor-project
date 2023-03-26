using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public EnemySpawner enemyspawner;
    private void Start()
    {
        Vector3 SpawnPOS = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));
        PhotonNetwork.Instantiate(playerPrefab.name, SpawnPOS, Quaternion.identity);
    }
}
