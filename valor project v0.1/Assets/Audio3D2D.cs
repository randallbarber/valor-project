using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Audio3D2D : MonoBehaviourPun
{
    [SerializeField] AudioSource audioShot;
    void Awake()
    {
        if (photonView.IsMine)
        {
            audioShot.spatialBlend = 0f;
        }
    }
}
