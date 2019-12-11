using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public int numberOfKills;
    public int killsNeeded;

    public GameObject gate;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (numberOfKills >= killsNeeded)
        {
            gate.SetActive(false);
        }
    }


}
