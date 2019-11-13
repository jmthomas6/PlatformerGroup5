using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    //private Collider2D _col;

    [SerializeField]
    private float _acceleration, _speed, _slowLimit, _jumpVelocity, _attackCooldown;
    [SerializeField]
    private Collider2D _groundTrigger, attackTrigger;
    [SerializeField]
    private Animator _anim;

    private bool _grounded, _doubleJump, _inCombat;
    private float _attackTimer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _grounded = true;
        _doubleJump = false;
        _inCombat = false;
        _attackTimer = 0f;
    }

    private void Update()
    {
        _anim.SetBool("Grounded", _grounded);
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
                _grounded = false;
                _anim.SetTrigger("Jump");
            }
            else if (_doubleJump)
            {
                newVelocity += Vector2.up * _jumpVelocity;
                _doubleJump = false;
                _anim.SetTrigger("Jump");
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

        _attackTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Z) && _attackTimer > _attackCooldown)
        {
            Attack();
            _attackTimer = 0f;
        }

        UpdateAnim();

        //REMOVE LATER, REPLACE WITH DIST CHECK
        if (Input.GetKeyDown(KeyCode.X))
        {
            _inCombat = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            _grounded = true;
            _doubleJump = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            _grounded = false;
        }
    }

    private void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    private void UpdateAnim()
    {
        Vector2 vel = _rb.velocity;
        if (vel.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (vel.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (_grounded)
        {
            if (Mathf.Abs(vel.x) > 0.01f)
            {
                _anim.SetInteger("AnimState", 2);
            }
            else if (_inCombat)
            {
                _anim.SetInteger("AnimState", 1);
            }
            else
            {
                _anim.SetInteger("AnimState", 0);
            }
        }
    }
}