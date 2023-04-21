using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float timeToKillThySelf = 1f;
    void Start()
    {
        Destroy(gameObject, timeToKillThySelf);
    }
}
