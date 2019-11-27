using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtRemover : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RoomSpawnPoint"))
        {
            Debug.LogError("tagged Spawn Point");
            Destroy(gameObject);
        }
    }
}
