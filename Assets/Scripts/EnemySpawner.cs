using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyPooler enemyPooler;

    private void Start()
    {
        enemyPooler = EnemyPooler.Instance;
    }

    private void FixedUpdate()
    {
        enemyPooler.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
    }
}
