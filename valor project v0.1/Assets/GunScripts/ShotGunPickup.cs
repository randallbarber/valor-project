using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunPickup : MonoBehaviour
{
    public Transform cam;
    public Transform GunController;

    ShotGunScript SC;
    Animator animator;

    bool holdingGun;
    private void Awake()
    {
        cam = GameObject.Find("Player/Controller/Main Camera").transform;
        SC = GunController.GetComponent<ShotGunScript>();
        animator = GunController.GetComponent<Animator>();
    }
    public void Pickup()
    {
        GunController.SetParent(cam);
        GunController.localPosition = new Vector3();
        GunController.rotation = cam.rotation;
        SC.enabled = true;
        animator.enabled = true;
        holdingGun = true;
    }
    public void Drop()
    {
        GunController.SetParent(null);
        GunController.rotation = cam.rotation;
        SC.enabled = false;
        animator.enabled = false;
        holdingGun = false;
    }
    float gravity = -9.81f;
    Vector3 velocity;
    bool isGrounded;
    float groundDistance = 0.4f;
    public LayerMask GroundMask;
    public Transform groundCheck;
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
                GunController.position += velocity * Time.deltaTime;
            }
        }
    }
}
