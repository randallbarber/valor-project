using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun
{
    [HideInInspector] public bool Sprinting = false;
    [SerializeField] float speed = 8f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3;
    bool isGrounded;
    Vector3 velocity;

    [SerializeField] CharacterController movementController;
    [SerializeField] Camera cam;
    [SerializeField] AudioListener listener;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask GroundMask;

    GunController GunController;
    Animator animator;

    private void Awake()
    {
        if (photonView.IsMine == false)
        {
            cam.enabled = false;
            listener.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // check if isGrounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, GroundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            // moving
            float x = Input.GetAxis("Horizontal");//a,d
            float z = Input.GetAxis("Vertical");//w,s

            Vector3 move = transform.right * x + transform.forward * z;

            movementController.Move(move * speed * Time.deltaTime);//movement
            // animations
            if (Input.GetAxis("Vertical") != 0 && GunController != null || Input.GetAxis("Horizontal") != 0 && GunController != null)
            {
                if (GunController.aiming == false)
                {
                    if (Input.GetButton("Sprint")) // sprinting
                    {
                        Sprinting = true;
                        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90, 5 * Time.deltaTime);
                        speed = 12f;
                        animator.SetBool("IsWalking", true);
                        animator.SetBool("IsSprinting", true);
                    }
                    else
                    {
                        Sprinting = false;
                        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 5 * Time.deltaTime);
                        speed = 8f;
                        animator.SetBool("recoilToWalk", true);
                        animator.SetBool("IsSprinting", false);
                        animator.SetBool("IsWalking", true);
                    }
                    
                }
                else // if aiming
                {
                    speed = 4f;
                    animator.SetBool("recoilToWalk", false);
                    animator.SetBool("IsWalking", false);
                }
            }
            else if (GunController != null) // walking 
            {
                    animator.SetBool("recoilToWalk", false);
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsSprinting", false);
            }
            // jumping
            if (Input.GetButtonDown("Jump") & isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

            }
            // gravity
            velocity.y += gravity * Time.deltaTime;
            movementController.Move(velocity * Time.deltaTime);
        }
    }
    public void SetController()
    {
        GunController = cam.GetComponentInChildren<GunController>();
        animator = cam.GetComponentInChildren<Animator>();
    }
}
