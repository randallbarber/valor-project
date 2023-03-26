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
    bool collidedWithOpening;
    float closestDistanceToPlayer = float.MaxValue;
    private void Update()
    {      
        if (TargetPlayer)
        {
            agentDestination = TargetPlayer.position;
        }

            NavAgent.destination = agentDestination;
        if (openingSC == true && foundPlayer == false)
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
    }
    private void OnTriggerStay(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health && Time.time >= NextTimeToFire)
        {
            NextTimeToFire = Time.time + 1f;
            health.TakeDamage(damage);
        }

        openingSC = other.GetComponent<Opening>();
        if (openingSC && Time.time >= NextTimeToFire)
        {
            NextTimeToFire = Time.time + 1f;
            openingSC.TakeDamage(damage);
        }
    }
}
