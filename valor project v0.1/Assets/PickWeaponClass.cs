using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickWeaponClass : MonoBehaviourPun
{
    [SerializeField] Canvas ClassCanvas;
    public void EnableWeaponChoice()
    {
        ClassCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void AssaultClassPicked()
    {
        ClassCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject GunClone = PhotonNetwork.Instantiate("M16Controller", Vector3.zero, Quaternion.identity);
        ItemPickup pickup = GunClone.GetComponent<ItemPickup>();
        pickup.Pickup();
    }
    public void ShotgunClassPicked()
    {
        ClassCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject GunClone = PhotonNetwork.Instantiate("pumpShotgunController", Vector3.zero, Quaternion.identity);
        ItemPickup pickup = GunClone.GetComponent<ItemPickup>();
        pickup.Pickup();
    }
    public void SMGClassPicked()
    {
        ClassCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject GunClone = PhotonNetwork.Instantiate("mp5Controller", Vector3.zero, Quaternion.identity);
        ItemPickup pickup = GunClone.GetComponent<ItemPickup>();
        pickup.Pickup();
    }
    public void MarksmanClassPicked()
    {
        ClassCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject GunClone = PhotonNetwork.Instantiate("m9berretaController", Vector3.zero, Quaternion.identity);
        ItemPickup pickup = GunClone.GetComponent<ItemPickup>();
        pickup.Pickup();
    }
}
