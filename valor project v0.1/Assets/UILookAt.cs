using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    public Canvas canvas;
    Transform player;
    bool foundPlayer;
    public void Start()
    {
        Invoke("FindPlayer", 1f);
    }
    private void Update()
    {
        if (foundPlayer)
        {
            canvas.transform.LookAt(player.position);
        }
    }
    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        foundPlayer = true;
    }
}
