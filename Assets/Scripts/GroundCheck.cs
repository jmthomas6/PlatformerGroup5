using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool grounded = false;
    public Transform col;
    
    [SerializeField]
    private LayerMask _includeLayers;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & _includeLayers) != 0)
        {
            grounded = true;
            this.col = col.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & _includeLayers) != 0)
        {
            grounded = true;
            this.col = col.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & _includeLayers) != 0)
            grounded = false;
    }
}