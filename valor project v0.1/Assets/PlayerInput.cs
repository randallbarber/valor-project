using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask PickableItem;
    
    float range = 10f;
    float repairRange = 3f;
    float repairTime = 3f;
    float repairHealthAmount = 100f;
    bool HoldingGun = false;
    bool readyedUp;

    Coroutine holdRepairCoroutine;

    PlayerCount playercount;
    Opening opening;
    MenuUI menuUI;
    Canvas RepairCanvas;
    Animator repairProgress;
    private void Start()
    {
        playercount = GameObject.Find("GameController").GetComponent<PlayerCount>();
        menuUI = GameObject.Find("GameUI").GetComponent<MenuUI>();
        RepairCanvas = menuUI.transform.Find("RepairCanvas").GetComponent<Canvas>();
        repairProgress = RepairCanvas.GetComponentInChildren<Animator>();
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
                    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, PickableItem))
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
            if (Input.GetButtonDown("Repair"))
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, repairRange))
                {
                    opening = hit.transform.GetComponent<Opening>();
                    if (opening)
                    {
                        holdRepairCoroutine = StartCoroutine(HoldRepair());
                        repairProgress.Play("repairProgress");
                    }
                }
            }
            if (Input.GetButtonUp("Repair"))
            {
                StopCoroutine(holdRepairCoroutine);
                repairProgress.Play("Default");
            }

            if (Input.GetButtonDown("ReadyUp") && readyedUp == false)
            {
                readyedUp = true;
                int ID = playercount.photonView.ViewID;
                photonView.RPC("playerReadyedUp", RpcTarget.AllBuffered, ID);
            }
            RaycastHit Barrierhit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out Barrierhit, repairRange))
            {
                if (Barrierhit.transform.tag == "Opening")
                {
                    RepairCanvas.enabled = true;
                }
            }
            else
            {
                RepairCanvas.enabled = false;
            }
        }
    }

    IEnumerator HoldRepair()
    {
        yield return new WaitForSeconds(repairTime);
        Debug.Log("repaired");
        opening.RepairHealth(repairHealthAmount);
    }
    [PunRPC]
    void playerReadyedUp(int PVID)
    {
        PhotonView PV = PhotonView.Find(PVID);
        playercount = PV.gameObject.GetComponent<PlayerCount>();
        playercount.PlayerReadyUp();
    }
}
