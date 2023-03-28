using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAgent : MonoBehaviourPun
{
    private NavMeshAgent NavAgent;

    private float damage = 10f;
    private float NextTimeToFire = 0f;

    public Transform attackArea;

    GameObject barrier;

    Health health;

    GameObject[] openings;
    private void Awake()
    {
        
        NavAgent = GetComponent<NavMeshAgent>();
    }

    float distance;
    float closestDistance = float.MaxValue;
    Vector3 agentDestination;
    private void Start()
    {
        openings = GameObject.FindGameObjectsWithTag("Opening");
        foreach (GameObject opening in openings)
        {
                distance = Vector3.Distance(transform.position, opening.transform.position);
                if (distance < closestDistance)
                {
                    agentDestination = opening.transform.position;
                    closestDistance = distance;
            }
        }
    }
    GameObject[] players;

    Opening openingSC;
    bool foundPlayer;
    Transform TargetPlayer;
    float closestDistanceToPlayer = float.MaxValue;

    float MaxDistance = 2f;

    private void Update()
    {
        if (NavAgent.isStopped == false)
        {
            if (Vector3.Distance(agentDestination, transform.position) <= MaxDistance)
            {
                NavAgent.isStopped = true;
            }
        }
        else
        { 
            if (Vector3.Distance(agentDestination, transform.position) >= MaxDistance)
            {
                NavAgent.isStopped = false;
            }
        }
        if (TargetPlayer)
        {
            agentDestination = TargetPlayer.position;
        }
        if (barrier && foundPlayer == false)
        {
            if (openingSC.dead == true)
            {
                foundPlayer = true;
                players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    distance = Vector3.Distance(transform.position, player.transform.position);
                    if (distance < closestDistanceToPlayer)
                    {
                        TargetPlayer = player.transform;
                        closestDistanceToPlayer = distance;
                    }
                }
            }
        }
        NavAgent.destination = agentDestination;
    }
    private void OnTriggerStay(Collider other)
    {
            health = other.GetComponent<Health>();
        if (health && Time.time >= NextTimeToFire)
        {
            NextTimeToFire = Time.time + 1f;
            health.TakeDamage(damage);
        }

        if (barrier == false)
        {
            openingSC = other.GetComponent<Opening>();
        }
        if (openingSC && Time.time >= NextTimeToFire && foundPlayer == false)
        {
            barrier = openingSC.gameObject;
            NextTimeToFire = Time.time + 1f;
            openingSC.TakeDamage(damage);
        }
    }
}
