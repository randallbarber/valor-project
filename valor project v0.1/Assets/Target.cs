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

    bool dead = false;
    private void Awake()
    {
        enemySpawner = GameObject.Find("GameController").GetComponent<EnemySpawner>();
        moneyHandler = GameObject.Find("GameController").GetComponent<MoneyHandler>();
    } 
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.LogError(health);
        if (health <= 0f && dead == false)
        {
            dead = true;
            die();
        }
    }
    void die()
    {
        hurt.Play();
        EnemyAgentSC.enabled = false;
        agent.enabled = false;
        collisionAI.isTrigger = false;
        rb.isKinematic = false;
        rb.velocity = new Vector3(0, 0, 0);

        gameObject.tag = "Untagged";

        moneyHandler.addMoney(amount);
        enemySpawner.EnemyKilled();
        //Destroy(gameObject, 5f);
    }
}
