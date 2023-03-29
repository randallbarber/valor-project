using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 100f;
    public AudioSource hurt;
    public float amount = 10f;

    public Rigidbody rb;
    public EnemyAgent EnemyAgentSC;
    public NavMeshAgent agent;
    public Collider collisionAI;

    public EnemySpawner enemySpawner;
    public MoneyHandler moneyHandler;

    public GameObject head;

    bool dead = false;

    public Animator animator;

    Rigidbody[] bodyParts;
    Collider[] colliders;
    private void Awake()
    {
        bodyParts = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        foreach (Rigidbody part in bodyParts)
        {
            part.isKinematic = true;
        }
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
        enemySpawner = GameObject.Find("GameController").GetComponent<EnemySpawner>();
        moneyHandler = GameObject.Find("GameController").GetComponent<MoneyHandler>();

    } 
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f && dead == false)
        {
            dead = true;
            die();
        }
    }
    void die()
    {
        hurt.Play();
        animator.enabled = false;
        EnemyAgentSC.enabled = false;
        agent.enabled = false;
        bodyParts = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        foreach (Rigidbody part in bodyParts)
        {
            part.isKinematic = false;
        }
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = false;
        }
        Destroy(collisionAI);
        Destroy(rb);
        Destroy(head);

        gameObject.tag = "Untagged";

        moneyHandler.addMoney(amount);
        enemySpawner.EnemyKilled();
        Destroy(gameObject, 5f);
    }
}
