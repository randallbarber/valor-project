using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayersPVP : MonoBehaviourPun
{
    [SerializeField] PickWeaponClass weaponClassChoice;
    void Start()
    {
        SpawnAPlayer();
    }
    public void SpawnAPlayer()
    {
        Vector3 SpawnPOS = new Vector3(0, 2, 0);
        int SpawnLocation = Random.Range(1, 3);
        if (SpawnLocation == 1)
        {
            SpawnPOS.x = Random.Range(0f, 34f);
            SpawnPOS.z = Random.Range(20f, 30f);
        }
        if (SpawnLocation == 2)
        {
            SpawnPOS.x = Random.Range(0f, 34f);
            SpawnPOS.z = Random.Range(-35f, -25f);
        }
        PhotonNetwork.Instantiate("Player", SpawnPOS, Quaternion.identity);
        weaponClassChoice.EnableWeaponChoice();
    }
}
