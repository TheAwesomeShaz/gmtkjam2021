using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // if (other.GetComponent<HoverBoard>() && other.GetComponent<HoverBoard>().isAlive)
        if (other.GetComponent<HoverBoard>())
        {
            enemy.playerInRange = true;
            enemy.target = other.transform;
        }

        if (other.GetComponent<Bullet>() && other.GetComponent<Bullet>().isDeflected)
        {
            enemy.Die();
        }
    }
}
