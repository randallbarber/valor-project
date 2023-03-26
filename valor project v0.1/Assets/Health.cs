using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    [HideInInspector] public bool dead;
    [SerializeField] float health = 100000f;

    Canvas DeadGUI;
    MenuUI menuUI;
    Animator animator;

    private void Start()
    {
        DeadGUI = GameObject.Find("GameUI/Game Over").GetComponent<Canvas>();
        menuUI = GameObject.Find("GameUI").GetComponent<MenuUI>();
        animator = GameObject.Find("GameUI/HurtOverlay/Panel").GetComponent<Animator>();
    }
    public void TakeDamage(float amount)
    {
        if (photonView.IsMine)
        {
            health -= amount;
            animator.Play("hurtOverlayAnim");
            if (health <= 0f && dead == false)
            {
                dead = true;
                die();
            }
        }
    }

    void die()
    {
        DeadGUI.enabled = true;
        menuUI.GameEnded();
    }
}
