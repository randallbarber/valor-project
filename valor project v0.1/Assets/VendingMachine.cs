using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VendingMachine : MonoBehaviour
{
    [SerializeField] bool pistol;
    [SerializeField] bool shotgun;
    [SerializeField] bool m16;
    [SerializeField] MoneyHandler wallet;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject shotgunGO;
    
    public void Clicked()
    {
        if (pistol == true)
        {
            if (wallet.money >= 10)
            {
                wallet.addMoney(-10);
                Instantiate(gun, transform.position, transform.rotation);
            }
        }
        if (shotgun == true)
        {
            if (wallet.money >= 50)
            {
                wallet.addMoney(-50);
                Instantiate(shotgunGO, transform.position, transform.rotation);
            }
        }
        if (m16 == true)
        {
            PhotonNetwork.Instantiate("M16Controller", transform.position + transform.forward, Quaternion.identity);
        }
    }
}
