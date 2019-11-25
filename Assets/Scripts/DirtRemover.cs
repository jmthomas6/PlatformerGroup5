using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtRemover : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.LogError("tagged wall");
            Destroy(gameObject);
        }
    }
}
