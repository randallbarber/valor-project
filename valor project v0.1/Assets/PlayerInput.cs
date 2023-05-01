using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask ItemLayer;
    
    float range = 10f;
    bool HoldingGun = false;

    MenuUI menuUI;
    private void Start()
    {
        menuUI = GameObject.Find("GameUI").GetComponent<MenuUI>();
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Escape")) // Menu
            {
                menuUI.OpenMenu();
            }
            if (Input.GetButtonDown("Drop")) // Drop
            {
                if (HoldingGun == true)
                {
                    HoldingGun = false;
                    ItemPickup item = cam.GetComponentInChildren<ItemPickup>();
                    if (item)
                    {
                        item.Drop();
                    }
                }
            }
            if (Input.GetButtonDown("Fire1")) // pickup
            {
                if (HoldingGun == false)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, ItemLayer))
                    {
                        ItemPickup gun = hit.transform.GetComponentInParent<ItemPickup>();
                        if (gun != null)
                        {
                            HoldingGun = true;
                            gun.Pickup();
                        }
                        VendingMachine store = hit.transform.GetComponent<VendingMachine>();
                        if (store != null)
                        {
                            store.Clicked();
                        }
                    }
                }
            }
        }
    }
}
