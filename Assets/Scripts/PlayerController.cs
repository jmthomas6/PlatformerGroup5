using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _col;

    [SerializeField]
    private float _acceleration, _speed, _slowLimit, _jumpVelocity;

    private bool _grounded, _doubleJump;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 newVelocity = _rb.velocity;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            newVelocity += Vector2.left * _acceleration;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            newVelocity += Vector2.right * _acceleration;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_grounded)
            {
                newVelocity += Vector2.up * _jumpVelocity;
            }
            else if (_doubleJump)
            {
                newVelocity += Vector2.up * _jumpVelocity;
                _doubleJump = false;
            }
        }

        if (newVelocity.x > _speed)
        {
            newVelocity.x = _speed;
        }
        if (newVelocity.x < -_speed)
        {
            newVelocity.x = -_speed;
        }
        if (newVelocity.x < _slowLimit && newVelocity.x > -_slowLimit)
        {
            newVelocity.x = 0;
        }
        _rb.velocity = newVelocity;
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