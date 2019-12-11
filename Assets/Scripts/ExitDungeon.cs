using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDungeon : MonoBehaviour
{
    UIController uIController;
    public GameObject endPanel;

    private void Start()
    {
        endPanel = GameObject.FindGameObjectWithTag("UIController");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uIController.GameOver();
        }
    }

}