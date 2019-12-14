using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDungeon : MonoBehaviour
{
    private UIController _gc;
    public GameObject endPanel;

    private void Start()
    {
        _gc = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponentInChildren<PlayerController>().dead = true;
            other.transform.GetComponent<Collider2D>().enabled = false;
            other.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            _gc.GameOver();
            _gc.VictoryMessage();
        }
    }
}