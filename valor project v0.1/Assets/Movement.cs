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
    [SerializeField] LayerMask GroundMask;
    float speed;
    bool isGrounded;
    Vector3 velocity;

    [Header("Transforms")]
    [SerializeField] Transform LeanController;
    [SerializeField] Transform groundCheck;
    [Header("Animators")]
    [SerializeField] Animator PlayerAnimator;
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
                if (GunController.aiming == false)
                {
                    PlayerAnimator.SetBool("IsJogging", true);
                    animator.SetBool("Walking", true);
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
                    animator.SetBool("Walking", false);
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
                animator.SetBool("Walking", false);
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

            if (Input.GetAxis("Horizontal") > 0)
            {
                //right
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                //left
            }
            if (Input.GetButton("LeanRight") || Input.GetButton("LeanLeft"))
            {
                HeadMesh.enabled = false;
                if (Input.GetButton("LeanRight"))
                {
                    LeanController.transform.localRotation = Quaternion.Lerp(LeanController.transform.localRotation, Quaternion.Euler(0f, 0f, -30f), LeanTime * Time.deltaTime);
                }
                if (Input.GetButton("LeanLeft"))
                {
                    LeanController.transform.localRotation = Quaternion.Lerp(LeanController.transform.localRotation, Quaternion.Euler(0f, 0f, 30f), LeanTime * Time.deltaTime);
                }
            }
            else
            {
                HeadMesh.enabled = true;
                LeanController.transform.localRotation = Quaternion.Lerp(LeanController.transform.localRotation, Quaternion.identity, LeanTime * Time.deltaTime);
            }
        }
    }
    public void SetController()
    {
        GunController = cam.GetComponentInChildren<GunController>();
        animator = GunController.GetComponentInChildren<Animator>();
    }
    IEnumerator FadeOutAudio(AudioSource audioTrack)
    {
        float _time = 0f;
        while (_time < 1f)
        {
            audioTrack.volume = Mathf.Lerp(audioTrack.volume, 0f, _time);
            _time += Time.deltaTime / 1f;
            yield return null;
        }
    }
}
