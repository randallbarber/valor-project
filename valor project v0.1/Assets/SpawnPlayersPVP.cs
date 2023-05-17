using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayersPVP : MonoBehaviourPun
{
    [SerializeField] PickWeaponClass weaponClassChoice;
    [SerializeField] Transform SpawnArea_1;
    [SerializeField] Transform SpawnArea_2;
    [SerializeField] float XRange;
    [SerializeField] float ZRange;
    [SerializeField] float Height;

    void Start()
    {
        SpawnAPlayer();
    }
    public void SpawnAPlayer()
    {
        Vector3 SpawnPOS = new Vector3();

        int SpawnLocation = Random.Range(1, 3);
        if (SpawnLocation == 1)
        {
            SpawnPOS = SpawnArea_1.position + new Vector3(Random.Range(-XRange, XRange), Height, Random.Range(-ZRange, ZRange));
        }
        if (SpawnLocation == 2)
        {
            SpawnPOS = SpawnArea_2.position + new Vector3(Random.Range(-XRange, XRange), Height, Random.Range(-ZRange, ZRange));
        }
        PhotonNetwork.Instantiate("Player", SpawnPOS, Quaternion.identity);
        weaponClassChoice.EnableWeaponChoice();
    }
}
