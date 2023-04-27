using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun
{
    [HideInInspector] public bool Sprinting = false;
    [SerializeField] float WalkSpeed = 8f;
    [SerializeField] float SprintSpeed = 8f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3;
    float speed;
    bool isGrounded;
    Vector3 velocity;

    [SerializeField] CharacterController movementController;
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] Camera cam;
    [SerializeField] AudioListener listener;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform FeetController;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask GroundMask;

    GunController GunController;
    Animator animator;

    private void Awake()
    {
        if (photonView.IsMine == false)
        {
            speed = WalkSpeed;
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

            if (Input.GetAxis("Vertical") != 0 && GunController != null || Input.GetAxis("Horizontal") != 0 && GunController != null) // walking
            {
                PlayerAnimator.SetBool("IsJogging", true);

                if (GunController.aiming == false)
                {
                    if (Input.GetButton("Sprint")) // sprinting
                    {
                        Sprinting = true;
                        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 80, 5 * Time.deltaTime);
                        speed = SprintSpeed;
                    }
                    else
                    {
                        Sprinting = false;
                        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 5 * Time.deltaTime);
                        speed = WalkSpeed;
                    }
                    
                }
                else // if aiming
                {
                    speed = WalkSpeed / 2;
                    animator.SetBool("IsWalking", false);
                }
            }
            else
            {
                PlayerAnimator.SetBool("IsJogging", false);
            }
            // jumping
            if (Input.GetButtonDown("Jump") & isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            // gravity
            velocity.y += gravity * Time.deltaTime;
            movementController.Move(velocity * Time.deltaTime);

            if (Input.GetAxis("Horizontal") > 0)
            {
                //right
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                //left
            }
        }
    }
    public void SetController()
    {
        GunController = cam.GetComponentInChildren<GunController>();
        animator = cam.GetComponentInChildren<Animator>();
    }
}
