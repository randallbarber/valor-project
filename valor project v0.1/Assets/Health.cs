using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    [HideInInspector] public bool dead;
    [SerializeField] float health;
    int killerID;
    bool firedFunction;
    float timeToFire;

    Canvas DeadGUI;
    MenuUI menuUI;
    Animator animator;
    Camera cam;
    setSpectator _setSpectator;
    SpawnPlayersPVP spawnPlayers;

    private void Start()
    {
        cam = Camera.main;
        spawnPlayers = GameObject.Find("AllClients").GetComponent<SpawnPlayersPVP>();
        DeadGUI = GameObject.Find("GameUI/Game Over").GetComponent<Canvas>();
        menuUI = GameObject.Find("GameUI").GetComponent<MenuUI>();
        animator = GameObject.Find("GameUI/HurtOverlay/Panel").GetComponent<Animator>();
    }
    public void SetKillerID(int ID)
    {
        killerID = ID;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        
        if (health <= 0f && dead == false)
        {
            dead = true;
        }
        if (photonView.IsMine)
        {
            animator.Play("hurtOverlayAnim");
            if (dead && firedFunction == false)
            {
                firedFunction = true;
                die();
            }
        }
    }

    void die()
    {
        PhotonView attackerPV = PhotonView.Find(killerID);
        _setSpectator = attackerPV.GetComponentInParent<setSpectator>();
        StartCoroutine(SpectatorView());
    }
    IEnumerator SpectatorView()
    {
        _setSpectator.SetSpectatorToPlayer();
        transform.position = new Vector3(0, -100, 0);
        yield return new WaitForSeconds(5f);
        _setSpectator.UnSetSpectatorToPlayer();
        PhotonNetwork.Destroy(gameObject);
        spawnPlayers.SpawnAPlayer();
    }
}
