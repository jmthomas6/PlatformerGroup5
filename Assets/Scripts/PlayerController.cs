using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GroundCheck _gc;
    //private Collider2D _col;

    [SerializeField]
    private float _acceleration, _speed, _slowLimit, _jumpVelocity, _attackCooldown, _climbSpeed;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private GroundCheck _climbTiggerBottom, _climbTriggerTop;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private GameObject _attackObj;
    [SerializeField]
    private SpriteRenderer _rend;
    [SerializeField]
    private List<Sprite> _climbFrames;
    [SerializeField]
    private Vector2 _damageVel;

    private bool _grounded, _doubleJump, _inCombat, _freezeMovement;
    private float _attackTimer;
    private float _baseScale;

    private void Start()
    {
        _gc = GetComponent<GroundCheck>();
        _grounded = true;
        _doubleJump = false;
        _inCombat = false;
        _freezeMovement = false;
        _attackTimer = 0f;
        _baseScale = _parent.localScale.x;
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;
        if (!_freezeMovement)
        {

            if (_gc.grounded)
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
                Climb();
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                newVelocity += Vector2.right * _acceleration;
                Climb();
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                // NEEDED?
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

            if (Input.GetKeyDown(KeyCode.X) && _attackTimer > _attackCooldown && _grounded)
            {
                StartCoroutine(Attack());
                _attackTimer = 0f;
            }

            UpdateAnim();

            //REMOVE LATER, REPLACE WITH DIST CHECK
            if (Input.GetKeyDown(KeyCode.C))
            {
                _inCombat = !_inCombat;
            }
        }
    }

    private void Climb()
    {
        if (_rb.velocity.y < 0 && _climbTiggerBottom.grounded && !_climbTriggerTop.grounded)
        {
            StartCoroutine("ClimbAnimation");
        }
    }

    private void UpdateAnim()
    {
        Vector2 vel = _rb.velocity;
        if (vel.x > 0)
        {
            _parent.localScale = new Vector3(-_baseScale, _baseScale, 1f);
        }
        else if (vel.x < 0)
        {
            _parent.localScale = new Vector3(_baseScale, _baseScale, 1f);
        }

        if (_grounded && Mathf.Abs(_rb.velocity.y) < 5)
        {
            if (Mathf.Abs(vel.x) > 0.05f)
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
        else
        {
            _anim.SetTrigger("Jump");
        }
    }

    private IEnumerator Attack()
    {
        _anim.SetTrigger("Attack");
        _freezeMovement = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.1f);

        _attackObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        _attackObj.SetActive(false);
        _freezeMovement = false;
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
        prevPos += new Vector3(-(_parent.transform.localScale.x * 0.5f), (_baseScale * 0.9f), 0f);
        _parent.transform.position = prevPos;
        yield return new WaitForSeconds(_climbSpeed);

        _rend.sprite = _climbFrames[3];
        yield return new WaitForSeconds(_climbSpeed);

        _rend.sprite = _climbFrames[4];
        yield return new WaitForSeconds(_climbSpeed);

        _rend.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        _rend.transform.localPosition = new Vector3(0f, 0f, 0f);
        _freezeMovement = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _anim.enabled = true;
        yield return null;
    }
}