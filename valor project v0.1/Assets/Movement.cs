using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun
{
    [HideInInspector] public bool Sprinting = false;
    [Header("Values")]
    [SerializeField] float WalkSpeed = 8f;
    [SerializeField] float SprintSpeed = 8f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] float LeanTime = 5f;
    [SerializeField] float strafeTime = 8f;
    [SerializeField] LayerMask GroundMask;
    float speed;
    bool isGrounded;
    bool mp_isMoving;
    bool prev_isMoving;
    Vector3 velocity;
    [Header("Objects")]
    [SerializeField] GameObject DummyModel;
    [SerializeField] GameObject MPDummyModel;
    [Header("Transforms")]
    [SerializeField] Transform camera_lean_target;
    [SerializeField] Transform model_lean_target;
    [SerializeField] Transform strafe_target;
    [SerializeField] Transform groundCheck;
    [Header("Animators")]
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] Animator MPlayerAnimator;
    [SerializeField] Animator CamAnimator;
    [Header("Components")]
    [SerializeField] CharacterController movementController;
    [SerializeField] Camera cam;
    [SerializeField] SkinnedMeshRenderer HeadMesh;
    [Header("Audio")]
    [SerializeField] AudioListener listener;
    [SerializeField] AudioSource WalkingSound;
    [SerializeField] AudioSource SlowWalkingSound;
    [SerializeField] AudioSource BreathingSound;
    [SerializeField] AudioSource FastBreathingSound;
    [SerializeField] AudioSource FastSprintingSound;

    GunController GunController;
    Animator GunAnimator;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            speed = WalkSpeed;
            cam.enabled = false;
            listener.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            Destroy(DummyModel);
            Destroy(CamAnimator.gameObject);
        }
        else
        {
            Destroy(MPDummyModel);
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
                velocity.y = 0f;
            }
            // moving
            float x = Input.GetAxis("Horizontal");//a,d
            float z = Input.GetAxis("Vertical");//w,s

            Vector3 move = transform.right * x + transform.forward * z;

            movementController.Move(move * speed * Time.deltaTime);//movement

            if (Input.GetAxis("Vertical") != 0 && GunController != null || Input.GetAxis("Horizontal") != 0 && GunController != null) // walking
            {
                PlayerAnimator.SetBool("IsJogging", true);
                mp_isMoving = true;
                if (GunController.aiming == false)
                {
                    GunAnimator.SetBool("Walking", true);
                    CamAnimator.SetBool("Walking", true);

                    BreathingSound.Stop();
                    SlowWalkingSound.Stop();

                    if (!FastBreathingSound.isPlaying)
                    {
                        FastBreathingSound.Play();
                    }
                    if (Input.GetButton("Sprint")) // sprinting
                    {
                        Sprinting = true;
                        GunAnimator.SetBool("Sprinting", true);
                        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 80, 5 * Time.deltaTime);
                        speed = SprintSpeed;
                        WalkingSound.Stop();
                        if (!FastSprintingSound.isPlaying)
                        {
                            FastSprintingSound.Play();
                        }
                    }
                    else
                    {
                        Sprinting = false;
                        GunAnimator.SetBool("Sprinting", false);
                        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 5 * Time.deltaTime);
                        speed = WalkSpeed;
                        FastSprintingSound.Stop();
                        if (!WalkingSound.isPlaying)
                        {
                            WalkingSound.Play();
                        }
                    }
                }
                else // if aiming
                {
                    speed = WalkSpeed / 2;
                    GunAnimator.SetBool("Walking", false);
                    CamAnimator.SetBool("Walking", false);

                    FastBreathingSound.Stop();
                    WalkingSound.Stop();
                    if (!SlowWalkingSound.isPlaying)
                    {
                        SlowWalkingSound.Play();
                    }
                    if (!BreathingSound.isPlaying)
                    {
                        BreathingSound.Play();
                    }
                }
            }
            else if (GunController != null)
            {
                PlayerAnimator.SetBool("IsJogging", false);
                mp_isMoving = false;
                GunAnimator.SetBool("Walking", false);
                CamAnimator.SetBool("Walking", false);
                WalkingSound.Stop();
                FastBreathingSound.Stop();
                SlowWalkingSound.Stop();
                if (!BreathingSound.isPlaying)
                {
                    BreathingSound.Play();
                }
            }
            // jumping
            if (Input.GetButtonDown("Jump") & isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            // gravity
            velocity.y += gravity * Time.deltaTime;
            movementController.Move(velocity * Time.deltaTime);

            if (Input.GetButton("LeanRight") || Input.GetButton("LeanLeft"))
            {
                if (Input.GetButton("LeanRight"))
                {
                    camera_lean_target.transform.localRotation = Quaternion.Lerp(camera_lean_target.transform.localRotation, Quaternion.Euler(0f, 0f, -30f), LeanTime * Time.deltaTime);
                    model_lean_target.transform.localRotation = Quaternion.Lerp(model_lean_target.transform.localRotation, Quaternion.Euler(0f, 0f, -30f), LeanTime * Time.deltaTime);
                }
                if (Input.GetButton("LeanLeft"))
                {
                    camera_lean_target.transform.localRotation = Quaternion.Lerp(camera_lean_target.transform.localRotation, Quaternion.Euler(0f, 0f, 30f), LeanTime * Time.deltaTime);
                    model_lean_target.transform.localRotation = Quaternion.Lerp(model_lean_target.transform.localRotation, Quaternion.Euler(0f, 0f, 30f), LeanTime * Time.deltaTime);
                }
            }
            else
            {
                camera_lean_target.transform.localRotation = Quaternion.Lerp(camera_lean_target.transform.localRotation, Quaternion.identity, LeanTime * Time.deltaTime);
                model_lean_target.transform.localRotation = Quaternion.Lerp(model_lean_target.transform.localRotation, Quaternion.identity, LeanTime * Time.deltaTime);
            }
            if (mp_isMoving != prev_isMoving)
            {
                if (mp_isMoving)
                {
                    photonView.RPC("UpdateMovingStatus", RpcTarget.Others, true);
                }
                else
                {
                    photonView.RPC("UpdateMovingStatus", RpcTarget.Others, false);
                }
                prev_isMoving = mp_isMoving;
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Horizontal") >= 0)
                {
                    strafe_target.localRotation = Quaternion.Lerp(strafe_target.localRotation, Quaternion.Euler(0f, 70f, 0f), strafeTime * Time.deltaTime);
                }
                if (Input.GetAxis("Horizontal") <= 0)
                {
                    strafe_target.localRotation = Quaternion.Lerp(strafe_target.localRotation, Quaternion.Euler(0f, -70f, 0f), strafeTime * Time.deltaTime);
                }
            }
            else
            {
                strafe_target.localRotation = Quaternion.Lerp(strafe_target.localRotation, Quaternion.identity, strafeTime * Time.deltaTime);
            }
        }
        if (!photonView.IsMine)
        {
            if (mp_isMoving)
            {
                MPlayerAnimator.SetBool("IsJogging", true);
            }
            else
            {
                MPlayerAnimator.SetBool("IsJogging", false);
            }
        }
    }
    public void SetController()
    {
        GunController = cam.GetComponentInChildren<GunController>();
        GunAnimator = GunController.GetComponentInChildren<Animator>();
    }
    [PunRPC]
    void UpdateMovingStatus(bool TrueOrFalse)
    {
        mp_isMoving = TrueOrFalse;
    }
}
