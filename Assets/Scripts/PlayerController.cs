using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _col;

    [SerializeField]
    private float _acceleration, _speed, _slowLimit, _jumpVel;

    private bool _grounded, _doubleJump;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 newVel = _rb.velocity;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            newVel += Vector2.left * _acceleration;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            newVel += Vector2.right * _acceleration;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_grounded)
            {
                newVel += Vector2.up * _jumpVel;
            }
            else if (_doubleJump)
            {
                newVel += Vector2.up * _jumpVel;
                _doubleJump = false;
            }
        }

        if (newVel.x > _speed)
        {
            newVel.x = _speed;
        }
        if (newVel.x < -_speed)
        {
            newVel.x = -_speed;
        }
        if (newVel.x < _slowLimit && newVel.x > -_slowLimit)
        {
            newVel.x = 0;
        }
        _rb.velocity = newVel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundTrigger"))
        {
            _grounded = true;
            _doubleJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundTrigger"))
        {
            _grounded = false;
        }
    }
}