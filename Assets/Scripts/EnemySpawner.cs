using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    private EnemyPooler enemyPooler;

    private int specialEnemyCountDown = 0;
    private int numberSpawnedEnemy1 = 0;
    private int numberSpawnedEnemy2 = 0;

    private void Start()
    {
        enemyPooler = EnemyPooler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (numberSpawnedEnemy1 <= 2)
            {
                enemyPooler.SpawnFromPool("Enemy", spawnPoint.position, Quaternion.identity);
                numberSpawnedEnemy1++;
            }

            specialEnemyCountDown++;
            if (specialEnemyCountDown == 5)
            {
                if(numberSpawnedEnemy2 < 1)
                {
                    enemyPooler.SpawnFromPool("Enemy2", spawnPoint.position, Quaternion.identity);
                    numberSpawnedEnemy2++;
                }
            }
        }
    }
}