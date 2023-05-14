using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    [SerializeField] Camera cam;
    
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
        }
    }
}
