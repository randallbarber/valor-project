using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Opening : MonoBehaviourPun
{
    [SerializeField] float health = 100f;
    public bool dead = false;

    MeshRenderer Mrenderer;
    Collider collider__;

    private void Start()
    {
        Mrenderer = GetComponent<MeshRenderer>();
        collider__ = GetComponent<Collider>();
    }
    public void TakeDamage(float amount)
    {
        if (dead == false)
        {
            health = Mathf.Clamp(health -= amount, 0, float.MaxValue);
            if (health == 0)
            {
                dead = true;
                Mrenderer.enabled = false;
                collider__.isTrigger = true;
                gameObject.layer = LayerMask.NameToLayer("Barrier");
            }
        }
    }
    public void RepairHealth(float amount)
    {
        photonView.RPC("RPC_HealthRepaired", RpcTarget.All, amount);
    }

    [PunRPC]
    void RPC_HealthRepaired(float RPCamount)
    {
        health = Mathf.Clamp(health += RPCamount, -float.MaxValue, 200f);
        Debug.Log("repaired to " + health);
        dead = false;
        Mrenderer.enabled = true;
        collider__.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }
}
