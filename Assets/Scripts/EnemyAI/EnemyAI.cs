using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private TriggerEvent triggerEvent;

    [SerializeField] private int damage = 20;
    [SerializeField] private float speed;
    [SerializeField] private float chaceRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCallDown;

    private float _timeSinceLastAttack;


    private EnemyState _CurrentState = EnemyState.Idle;
    private Health myHealth;
    private Health _playerHealth;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        myHealth = GetComponent<Health>();
        _playerHealth = FindObjectOfType<PlayerController>().GetComponent<Health>();
    }

    private void OnEnable()
    {
        myHealth.OnDie += () => _CurrentState = EnemyState.Dead;
        triggerEvent.OnTriggerInter += ApplyDamage;
    }



    private void OnDisable()
    {
        myHealth.OnDie -= () => _CurrentState = EnemyState.Dead;

    }

    private void FixedUpdate()
    {
        _timeSinceLastAttack += Time.deltaTime;

        if (_playerHealth == null || _playerHealth.IsDead()) return;
        switch (_CurrentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Chacing:
                HandleChaceState();
                break;
            case EnemyState.Attacking:
                HandleAttackState();
                break;
            case EnemyState.Dead:
                HandleDeadState();
                break;
        }
    }

    private void HandleDeadState()
    {
        Destroy(gameObject);
    }

    private void HandleIdleState()
    {

        if (Vector2.Distance(transform.position, _playerHealth.transform.position) < chaceRange)
        {
            _CurrentState = EnemyState.Chacing;
            return;
        }
        _rigidbody.velocity = Vector2.zero;
        _animator.SetFloat("Movement", 0);

    }

    private void HandleChaceState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _playerHealth.transform.position);

        if (distanceToPlayer > chaceRange)
        {
            _CurrentState = EnemyState.Idle;
            return;
        }
        if (distanceToPlayer < attackRange)
        {
            _CurrentState = EnemyState.Attacking;
            return;
        }

        _rigidbody.velocity = transform.position.x - _playerHealth.transform.position.x > 0 ? -transform.right * speed : transform.right * speed;
        _animator.SetFloat("Movement", 1);
    }

    private void HandleAttackState()
    {
        if (Vector2.Distance(transform.position, _playerHealth.transform.position) > attackRange)
        {
            _CurrentState = EnemyState.Idle;
        }
        _rigidbody.velocity = Vector2.zero;
        _animator.SetFloat("Movement", 0);

        if (_timeSinceLastAttack < attackCallDown)
        {
            return;
        }
        _animator.CrossFadeInFixedTime("Attack", .2f);
        _timeSinceLastAttack = 0;
    }
    public bool ShouldFaceLeft()
    {
        return transform.position.x - _playerHealth.transform.position.x > 0;
    }

    private void ApplyDamage(Collider2D obj)
    {
        if (obj.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaceRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


public enum EnemyState
{
    Idle,
    Chacing,
    Attacking,
    Dead
}
