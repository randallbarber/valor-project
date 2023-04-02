using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setSpectator : MonoBehaviour
{
    [SerializeField] Camera SpectatorCam;
    public void SetSpectatorToPlayer()
    {
        Camera.main.enabled = false;
        SpectatorCam.enabled = true;
    }
    public void UnSetSpectatorToPlayer()
    {
        SpectatorCam.enabled = false;
    }
}
