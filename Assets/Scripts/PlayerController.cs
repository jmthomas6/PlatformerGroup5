﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Objects")]
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private SpriteRenderer _rend;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private GroundCheck _climbTiggerBottom, _climbTriggerTop, _attackObj;

    [Header("Movement")]
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _slowLimit;
    [SerializeField]
    private float _jumpVelocity;
    [SerializeField]
    private float _climbSpeed;
    [SerializeField]
    private float _climbHeight;
    [SerializeField]
    private List<Sprite> _climbFrames;

    [Header("Combat")]
    [SerializeField]
    private Vector2 _damageVel;
    [SerializeField]
    private int _health;
    [SerializeField]
    private float _attackCooldown;

    private bool _grounded, _doubleJump, _invulnerable, _freezeMovement, _attackWindow;
    private float _attackTimer;
    private float _baseScale;
    private UIController _gc;
    private GroundCheck _triggerCheck;
    private Coroutine _attackAnim;

    [HideInInspector]
    public bool dead;
    #endregion

    private void Start()
    {
        _triggerCheck = GetComponent<GroundCheck>();
        _gc = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        _grounded = true;
        _doubleJump = false;
        _invulnerable = false;
        _freezeMovement = false;
        _attackTimer = 0f;
        _baseScale = _parent.localScale.x;
    }

    private void Update()
    {
        if (!_gc.isPaused && !dead)
        {
            _attackTimer += Time.deltaTime;
            if (!_freezeMovement && _gc.gameStarted)
            {

                if (_triggerCheck.grounded)
                {
                    _grounded = true;
                    _doubleJump = true;
                }
                else
                {
                    _grounded = false;
                }

                _anim.SetBool("Grounded", _grounded);
                Vector2 newVelocity = _rb.velocity;

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    newVelocity += Vector2.left * _acceleration;
                    if (_rb.velocity.y < 0 && _climbTiggerBottom.grounded && !_climbTriggerTop.grounded)
                        StartCoroutine("ClimbAnimation");
                }

                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    newVelocity += Vector2.right * _acceleration;
                    if (_rb.velocity.y < 0 && _climbTiggerBottom.grounded && !_climbTriggerTop.grounded)
                        StartCoroutine("ClimbAnimation");
                }

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
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
                    newVelocity.x = _speed;

                if (newVelocity.x < -_speed)
                    newVelocity.x = -_speed;

                if (newVelocity.x < _slowLimit && newVelocity.x > -_slowLimit)
                    newVelocity.x = 0;

                _rb.velocity = newVelocity;

                if (Input.GetKeyDown(KeyCode.X) && _attackTimer > _attackCooldown && _grounded)
                {
                    _attackAnim = StartCoroutine(AttackAnim());
                    _attackTimer = 0f;
                }

                UpdateAnim();

                // CHEAT CODE
                if (Input.GetKeyDown(KeyCode.C))
                    _invulnerable = !_invulnerable;
            }

            if (_attackWindow && _attackObj.grounded)
            {
                _attackWindow = false;
                _attackObj.col.gameObject.GetComponentInChildren<EnemyController>().Damage(new Vector2(_damageVel.x * Mathf.Sign(_parent.localScale.x), _damageVel.y));
            }
        }
    }

    private void UpdateAnim()
    {
        Vector2 vel = _rb.velocity;
        if (vel.x > 0)
            _parent.localScale = new Vector3(-_baseScale, _baseScale, 1f);
        else if (vel.x < 0)
            _parent.localScale = new Vector3(_baseScale, _baseScale, 1f);

        if (_grounded && Mathf.Abs(_rb.velocity.y) < 5)
        {
            if (Mathf.Abs(vel.x) > 0.05f)
                _anim.SetInteger("AnimState", 2);
            else if (_invulnerable)
                _anim.SetInteger("AnimState", 1);
            else
                _anim.SetInteger("AnimState", 0);
        }
        else
        {
            _anim.SetTrigger("Jump");
        }
    }

    private IEnumerator AttackAnim()
    {
        _anim.SetTrigger("Attack");
        _freezeMovement = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.1f);

        _attackWindow = true;
        yield return new WaitForSeconds(0.2f);

        _attackWindow = false;
        _freezeMovement = false;

        if (!dead)
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return null;
    }

    private IEnumerator ClimbAnimation()
    {
        _freezeMovement = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _anim.enabled = false;
        Vector3 prevPos = this._parent.transform.position;

        _rend.sprite = _climbFrames[0];
        _rend.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        _rend.transform.localPosition = new Vector3(0.25f, -0.1f, 0f);
        yield return new WaitForSeconds(_climbSpeed);

        _rend.sprite = _climbFrames[1];
        _rend.transform.localRotation = Quaternion.Euler(0f, 0f, -45f);
        _rend.transform.localPosition = new Vector3(0.1f, 0.5f, 0f);
        yield return new WaitForSeconds(_climbSpeed);

        _rend.sprite = _climbFrames[2];
        _rend.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        _rend.transform.localPosition = new Vector3(0f, 0f, 0f);
        prevPos += new Vector3(-(_parent.transform.localScale.x * 0.5f), _climbHeight, 0f);
        _parent.transform.position = prevPos;
        yield return new WaitForSeconds(_climbSpeed);

        _rend.sprite = _climbFrames[3];
        yield return new WaitForSeconds(_climbSpeed);

        _rend.sprite = _climbFrames[4];
        yield return new WaitForSeconds(_climbSpeed);

        _rend.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        _rend.transform.localPosition = new Vector3(0f, 0f, 0f);
        _freezeMovement = false;
        if (!dead)
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        _anim.enabled = true;
        yield return null;
    }

    public void Damage(Vector2 flinchDirection)
    {
        if (!_invulnerable)
        {
            _health--;
            _gc.LoseHeart(_health);

            if (_health > 0)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                Vector2 vel = _rb.velocity;
                vel += flinchDirection;
                _rb.velocity = vel;
                dead = false;
            }
            else if (_health == 0)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _parent.transform.GetComponent<Collider2D>().enabled = false;
                _anim.SetTrigger("Recover");
                dead = true;
                _gc.GameOver();
                _gc.DefeatMessage();
            }
        }
    }
}