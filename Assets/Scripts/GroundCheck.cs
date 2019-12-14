using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool grounded = false;
    public Transform col;
    [SerializeField]
    private string tagCheck;
    [SerializeField]
    private LayerMask _includeLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _includeLayers) != 0)
        {
            //if (collision.CompareTag(tagCheck))
            {
                grounded = true;
                col = collision.transform;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _includeLayers) != 0)
        {
            //if (collision.CompareTag(tagCheck))
            {
                grounded = true;
                col = collision.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _includeLayers) != 0)
        {
            //if (collision.CompareTag(tagCheck))
            {
                grounded = false;
            }
        }
    }
}