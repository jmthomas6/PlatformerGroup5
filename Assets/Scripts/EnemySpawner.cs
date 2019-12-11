using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyPooler enemyPooler;
    public int specialEnemyCountDown = 0;

    public int numberSpawnedEnemy1 = 0;
    public int numberSpawnedEnemy2 = 0;

    private void Start()
    {
        enemyPooler = EnemyPooler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (numberSpawnedEnemy1 <= 2)
            {
                enemyPooler.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
                numberSpawnedEnemy1++;
            }
            specialEnemyCountDown++;
            if (specialEnemyCountDown == 5)
            {
                if(numberSpawnedEnemy2 < 1)
                {
                    enemyPooler.SpawnFromPool("Enemy2", transform.position, Quaternion.identity);
                    numberSpawnedEnemy2++;
                }
                
            }
            
        }
    }
}
