using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private float _acceleration, _speed, _slowLimit, _attackCooldown, _attackRange, _combatRange, _waitMin, _waitMax, _combatTimer;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private GroundCheck _ledgeChecker;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private GameObject _attackObj;
    [SerializeField]
    private SpriteRenderer _rend;
    [SerializeField]
    private Vector2 _damageVel, _ledgeCheckOffset;

    private GroundCheck _gc;
    private bool _grounded, _inCombat, _freezeMovement;
    private float _attackTimer, _baseScale, _timeOutCombat;
    private RaycastHit2D _playerCheck, _obstacleCheck;
    private int _layerMask;
    private Coroutine _patrolling, _pursuing;

    private void Start()
    {
        _gc = GetComponent<GroundCheck>();
        _grounded = true;
        _inCombat = false;
        _freezeMovement = false;
        _attackTimer = 0f;
        _baseScale = _parent.localScale.x;

        _layerMask = ~(LayerMask.GetMask("Enemies"));
        _playerCheck = Physics2D.Raycast(transform.position, Vector2.left * Mathf.Sign(_parent.localScale.x), _combatRange, _layerMask);
        _patrolling = StartCoroutine(Patrol());
    }

    private void FixedUpdate()
    {
        _attackTimer += Time.deltaTime;
        _playerCheck = Physics2D.Raycast(transform.position, Vector2.left * Mathf.Sign(_parent.localScale.x), _combatRange, _layerMask);
        _obstacleCheck = Physics2D.Raycast(transform.position, Vector2.left * Mathf.Sign(_parent.localScale.x), _attackRange, _layerMask);
        if (_playerCheck.collider != null && _playerCheck.collider.transform.tag == "Player" && !_inCombat)
        {
            _inCombat = true;
            if (_pursuing != null)
            {
                StopCoroutine(_pursuing);
            }
            _pursuing = StartCoroutine(Pursue());
            _timeOutCombat = 0f;
        }
        else if ((_playerCheck.collider == null || (_playerCheck.collider != null && _playerCheck.collider.transform.tag != "Player")) && _timeOutCombat < _combatTimer)
        {
            _timeOutCombat += Time.deltaTime;
        }
        if (_timeOutCombat >= _combatTimer && _inCombat)
        {
            _inCombat = false;
            if (_patrolling != null)
            {
                StopCoroutine(_patrolling);
            }
            _patrolling = StartCoroutine(Patrol());
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) // REMOVE AFTER TESTING
        {
            Damage(new Vector2(25f, 25f));
        }
        UpdateAnim();
    }

    private void UpdateAnim()
    {
        Vector2 vel = _rb.velocity;
        if (vel.x > _slowLimit + 1)
        {
            _parent.localScale = new Vector3(-_baseScale, _baseScale, 1f);
        }
        else if (vel.x < -(_slowLimit + 1))
        {
            _parent.localScale = new Vector3(_baseScale, _baseScale, 1f);
        }
        
        if (_gc.grounded && Mathf.Abs(_rb.velocity.y) < 5)
        {
            _anim.SetBool("Grounded", true);
            if (Mathf.Abs(vel.x) > _slowLimit)
            {
                _anim.SetInteger("AnimState", 2);
                //print("state 2");
            }
            else if (_inCombat)
            {
                _anim.SetInteger("AnimState", 1);
                //print("state 1");
            }
            else
            {
                _anim.SetInteger("AnimState", 0);
                //print("state 0");
            }
        }
        else
        {
            _anim.SetTrigger("Jump");
        }
    }

    private IEnumerator Patrol()
    {
        print("patrolling!");
        while (true)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            
            float waitTimer = Random.Range(_waitMin, _waitMax);
            float t = 0;
            while (t < waitTimer && !_inCombat)
            {
                t += Time.deltaTime;
                yield return null;
            }
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (_inCombat)
            {
                break;
            }
            
            _parent.localScale = new Vector3(-_parent.localScale.x, _parent.localScale.y, 1f);
            yield return null;
            
            while (true)
            {
                Vector2 newVelocity = _rb.velocity;
                newVelocity += Vector2.left * _acceleration * Mathf.Sign(_parent.localScale.x);
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
                yield return null;
                
                yield return new WaitForEndOfFrame();
                if (!_ledgeChecker.grounded || _obstacleCheck.collider != null || _inCombat)
                {
                    _rb.velocity = Vector2.zero;
                    break;
                }
            }
        }
        yield return null;
    }

    private IEnumerator Pursue()
    {
        print("pursuing!");
        while (_inCombat)
        {
            while (true) // Run
            {
                Vector2 newVelocity = _rb.velocity;
                newVelocity += Vector2.left * _acceleration * Mathf.Sign(_parent.localScale.x);
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
                yield return null;
                
                yield return new WaitForEndOfFrame();
                while (_obstacleCheck.collider != null && _obstacleCheck.collider.transform.tag == "Player")
                {
                    _rb.velocity = Vector2.zero;
                    _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    StartCoroutine(Attack());
                    yield return new WaitForSeconds(_attackCooldown);
                }
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (!_ledgeChecker.grounded || (_obstacleCheck.collider != null && _obstacleCheck.collider.transform.tag != "Player"))
                {
                    _rb.velocity = Vector2.zero;
                    _parent.localScale = new Vector3(-_parent.localScale.x, _parent.localScale.y, 1f);
                    break;
                }
            }
            yield return null;
        }
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return null;
    }

    private IEnumerator Attack()
    {
        print("attacking!");
        _anim.SetTrigger("Attack");
        _freezeMovement = true;
        yield return new WaitForSeconds(0.1f);

        _attackObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        _attackObj.SetActive(false);
        _freezeMovement = false;
        yield return null;
    }

    public void Damage(Vector2 flinchDirection)
    {
        if (_patrolling != null)
        {
            StopCoroutine(_patrolling);
        }
        if (_pursuing != null)
        {
            StopCoroutine(_pursuing);
        }
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 vel = _rb.velocity;
        vel += flinchDirection;
        _rb.velocity = vel;
        StartCoroutine(Startled());
    }

    private IEnumerator Startled()
    {
        yield return new WaitForSeconds(0.25f);
        while (!_gc.grounded)
        {
            yield return null;
        }
        _inCombat = true;
        _timeOutCombat = 0f;
        if (_pursuing != null)
        {
            StopCoroutine(_pursuing);
        }
        _pursuing = StartCoroutine(Pursue());
        yield return null;
    }
    /*
    private bool LedgeCheck()
    {
        Vector3 endPos = _ledgeCheckOffset;
        endPos.x *= Mathf.Sign(_parent.localScale.x);
        RaycastHit2D ledgeRay = Physics2D.Linecast(transform.position, transform.position + endPos, _layerMask);
        if ()
    }*/
}