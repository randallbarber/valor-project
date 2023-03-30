using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VendingMachine : MonoBehaviour
{
    [SerializeField] bool pistol;
    [SerializeField] bool shotgun;
    [SerializeField] bool m16;
    [SerializeField] bool m9b;
    
    public void Clicked()
    {
        if (m16 == true)
        {
            PhotonNetwork.Instantiate("M16Controller", transform.position + transform.forward, Quaternion.identity);
        }
        if (m9b == true)
        {
            PhotonNetwork.Instantiate("m9berretaController", transform.position + transform.forward, Quaternion.identity);
        }
    }
}
