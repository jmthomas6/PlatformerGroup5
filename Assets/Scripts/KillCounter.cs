using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public int numberOfKills;
    public int killsNeeded;

    public GameObject attackedTrigger;

    EnemyPooler enemyPooler;
    /*
    private void Start()
    {
        enemyPooler = EnemyPooler.Instance;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
    } */
}
