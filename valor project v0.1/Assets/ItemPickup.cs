using UnityEngine;
using Photon.Pun;

public class ItemPickup : MonoBehaviourPun
{
    [SerializeField] Collider collisionDetector;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] Transform gunModel;
    [SerializeField] Transform LHT;
    [SerializeField] Transform RHT;

    Transform cam;
    GunController SC;
    Movement movement;
    Animator animator;
    Transform groundCheck;
    Transform viewmodel;

    float gravity = -9.81f;
    Vector3 velocity;
    bool isGrounded;
    bool holdingGun;
    public void Pickup()
    {
        cam = Camera.main.gameObject.transform;
        SC = gameObject.GetComponent<GunController>();
        movement = cam.GetComponentInParent<Movement>();
        animator = gunModel.GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        viewmodel = cam.transform.GetComponentInChildren<SetIK_Targets>().transform;

        transform.SetParent(viewmodel);
        viewmodel.GetComponent<SetIK_Targets>().SetTargets(LHT, RHT);
        holdingGun = true;
        animator.enabled = true;
        SC.enabled = true;
        collisionDetector.enabled = false;

        int targetPlayerID = movement.GetComponent<PhotonView>().ViewID;
        photonView.RPC("rpc_SetParent", RpcTarget.OthersBuffered,targetPlayerID, true);

        photonView.RequestOwnership();
        movement.SetController();
    }
    public void Drop()
    {}
    [PunRPC]
    public void rpc_SetParent(int targetPLYR, bool Pickup)
    {
        SC = gameObject.GetComponent<GunController>();
        animator = gunModel.GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");

        if (Pickup)
        {
            PhotonView tarPlyPV = PhotonView.Find(targetPLYR);
            Transform _parent = tarPlyPV.transform.Find("MP_Dummy").GetComponentInChildren<SetIK_Targets>().transform;
            
            transform.SetParent(_parent);
            _parent.GetComponentInChildren<SetIK_Targets>().SetTargets(LHT, RHT);
            holdingGun = true;
            animator.enabled = true;
            SC.enabled = true;
            collisionDetector.enabled = false;
        }
        else
        {
            //drop
        }
    }
    private void Update()
    {
        if (holdingGun == false)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, GroundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f;
            }
            if (isGrounded == false)
            {
                velocity.y += gravity * Time.deltaTime;
                transform.position += velocity * Time.deltaTime;
            }
        }
    }
}
