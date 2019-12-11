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
    private Vector2 _damageVel;

    private GroundCheck _gc;
    [SerializeField]
    private bool _grounded, _inCombat, _freezeMovement;
    private float _attackTimer, _baseScale, _timeOutCombat;
    private RaycastHit2D _ray;
    private int _layerMask;

    private void Start()
    {
        _gc = GetComponent<GroundCheck>();
        _grounded = true;
        _inCombat = false;
        _freezeMovement = false;
        _attackTimer = 0f;
        //_baseScale = _parent.localScale.x; // NEEDED?

        _layerMask = ~(LayerMask.GetMask("Enemies"));
        _ray = Physics2D.Raycast(transform.position, Vector2.left * Mathf.Sign(_parent.localScale.x), _combatRange, _layerMask);
        StartCoroutine(Patrol());
    }

    private void FixedUpdate()
    {
        _ray = Physics2D.Raycast(transform.position, Vector2.left * Mathf.Sign(_parent.localScale.x), _combatRange, _layerMask);
        if (_ray.collider != null && _ray.collider.transform.tag == "Player")
        {
            _inCombat = true;
            _timeOutCombat = 0f;
        }
        else if (_timeOutCombat < _combatTimer)
        {
            _timeOutCombat += Time.deltaTime;
        }
        UpdateAnim();
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

    private IEnumerator Patrol()
    {
        while (true)
        {
            float waitTimer = Random.Range(_waitMin, _waitMax);
            float t = 0;
            while (t < waitTimer && !_inCombat)
            {
                t += Time.deltaTime;
                yield return null;
            }
            if (_inCombat)
            {
                break;
            }
            _parent.localScale = new Vector3(-_parent.localScale.x, _parent.localScale.y, 1f);
        }
        yield return null;
    }
}
// wait random time
// flip
// run forward until ground check false
// repeat

// Raycast forward for player
// Run forward until distance is within range
// attack every timer reset