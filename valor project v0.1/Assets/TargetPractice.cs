using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPractice : MonoBehaviour
{
    float Tar_Health = 500f;
    public void TakeDamage(float damage)
    {
        Tar_Health -= damage;
        if (Tar_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
