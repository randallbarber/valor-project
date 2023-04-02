using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviourPun
{
    public static float MouseSens = 250;

    [SerializeField] Transform PlayerBody;

    float xRotation = 0f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float mouseX = Input.GetAxis("Mouse X") * MouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * MouseSens * Time.deltaTime;

            xRotation -= mouseY; //minus because input from mouse is negative
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);//clamps so value cant excede or dip below

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);//rotates only cameras X(up and down) because you don't want player to move up and down

            PlayerBody.Rotate(Vector3.up * mouseX);//rotates player and camera(player is parented)
        }
    }
}
