using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyPooler enemyPooler;
    public int specialEnemyCountDown = 0;

    private void Start()
    {
        enemyPooler = EnemyPooler.Instance;
    }

    private void FixedUpdate()
    {
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyPooler.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
            specialEnemyCountDown++;
            if (specialEnemyCountDown == 2)
            {
                enemyPooler.SpawnFromPool("Enemy2", transform.position, Quaternion.identity);
                specialEnemyCountDown = 0;
            }
            
        }
    }
}
