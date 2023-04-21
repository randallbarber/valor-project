using UnityEngine;
using Photon.Pun;

public class ItemPickup : MonoBehaviourPun
{
    [SerializeField] Collider collisionDetector;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] LayerMask GroundMask;

    Transform cam;
    GunController SC;
    Movement movement;
    Animator animator;
    Transform groundCheck;

    float gravity = -9.81f;
    Vector3 velocity;
    bool isGrounded;
    bool holdingGun;
    public void Pickup()
    {
        cam = Camera.main.gameObject.transform;
        SC = gameObject.GetComponent<GunController>();
        movement = cam.GetComponentInParent<Movement>();
        animator = gameObject.GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");

        int targetPlayerID = cam.Find("WeaponHolder").GetComponent<PhotonView>().ViewID;
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
        animator = gameObject.GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");

        if (Pickup)
        {
            PhotonView tarPlyPV = PhotonView.Find(targetPLYR);
            Transform _parent = tarPlyPV.gameObject.transform;
            transform.SetParent(_parent);
            holdingGun = true;
            animator.enabled = true;
            SC.enabled = true;
            collisionDetector.enabled = false;
            transform.rotation = cam.rotation;
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
