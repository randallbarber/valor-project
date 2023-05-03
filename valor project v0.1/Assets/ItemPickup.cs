using UnityEngine;
using Photon.Pun;

public class ItemPickup : MonoBehaviourPun
{
    [SerializeField] Collider collisionDetector;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] Transform gunModel;

    Transform cam;
    GunController SC;
    Movement movement;
    Animator animator;
    Transform groundCheck;

    Transform LHT;
    Transform RHT;

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

        LHT = GetComponentInChildren<Animator>().transform.Find("left_hand_target");
        RHT = GetComponentInChildren<Animator>().transform.Find("right_hand_target");

        int targetPlayerID = cam.transform.GetComponentInChildren<SetIK_Targets>().transform.GetComponent<PhotonView>().ViewID;
        photonView.RPC("RPC_SetParent", RpcTarget.AllBuffered, targetPlayerID, true);
        photonView.RequestOwnership();
        movement.SetController();
    }
    public void Drop()
    {
        photonView.RPC("RPC_SetParent", RpcTarget.AllBuffered, 0, false);
        movement.SetController();
    }
    [PunRPC]
    void RPC_SetParent(int targetPLYR, bool Pickup)
    {
        cam = Camera.main.gameObject.transform;
        SC = gameObject.GetComponent<GunController>();
        movement = cam.GetComponentInParent<Movement>();
        animator = gunModel.GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");

        LHT = GetComponentInChildren<Animator>().transform.Find("left_hand_target");
        RHT = GetComponentInChildren<Animator>().transform.Find("right_hand_target");
        if (Pickup)
        {
            PhotonView tarPlyPV = PhotonView.Find(targetPLYR);
            Transform _parent = tarPlyPV.gameObject.transform;
            transform.SetParent(_parent);
            _parent.GetComponent<SetIK_Targets>().SetTargets(LHT, RHT);
            holdingGun = true;
            animator.enabled = true;
            SC.enabled = true;
            collisionDetector.enabled = false;
        }
        else
        {
            transform.SetParent(null);
            transform.rotation = cam.rotation;
            SC.enabled = false;
            animator.enabled = false;
            collisionDetector.enabled = true;
            holdingGun = false;
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
