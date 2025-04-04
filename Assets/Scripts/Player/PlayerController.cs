using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public event Action OnPlayerJump;
    public event Action<bool> OnGroundChange;

    [Header("Movement Data")]
    [SerializeField] float maxSpeed;
    [SerializeField] float moveAcceleration;
    [SerializeField] float airDeceleration; // decleration
    [SerializeField] float groundDeceleration;


    [Header("Jump Data")]
    [SerializeField] float jumpPower;
    [SerializeField] int maxJumpCount;
    [SerializeField] float jumpBufferTime = .2f;
    [SerializeField] float coyoteTime = .15f;

    [Header("Gravity Data")]
    [SerializeField] float maxFallSpeed;
    [SerializeField] float fallAcceleration;
    [SerializeField] float groundingForce;
    [SerializeField] float jumpEndEarlyGravityModifier;

    [Header("Collison Data")]
    [SerializeField] float groundDistance;
    [SerializeField] float rad;
    [SerializeField] Vector3 topOffset;
    [SerializeField] Vector3 bottomOfsset;
    [SerializeField] LayerMask groundLayers;

    private AbilityBase[] _abilities;

    public bool CanControlPlayer
    {

        get
        {

            return _canControlPlayer;
        }
        set
        {
            if (_rb) _rb.velocity = Vector2.zero;
            _canControlPlayer = value;
        }
    }

    private Vector2 _velocity;
    private float _timeSinceLastJumpPressed;
    private float _timeSinceLeftGround;
    private int _lastPlayerDirection = 1;
    private int _jumpsLeft;

    private bool _canControlPlayer;
    private bool _isGrounded;
    private bool _endedJumpEarly;
    private bool _hasBufferdJump;
    private bool _canUseCoyote;
    private bool _canUlt;

    private PlayerControls _playerControls;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private MeatCollector _meatCollector;
    private MovingPlatform _platform;
    private Health _health;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = _rb.GetComponent<CapsuleCollider2D>();
        _playerControls = GetComponent<PlayerControls>();
        _meatCollector = GetComponent<MeatCollector>();
        _health = GetComponent<Health>();
        _abilities = GetComponents<AbilityBase>();
    }



    private void OnEnable()
    {
        _health.OnDie += Die;

        _playerControls.OnAbility1Use += () => _abilities[0].TriggerAbility();
        _playerControls.OnAbility2Use += () => _abilities[1].TriggerAbility();
        _playerControls.OnAbility3Use += () => _abilities[2].TriggerAbility();
        _meatCollector.OnFullCharge += () => _canUlt = true;
    }


    private void OnDisable()
    {
        _playerControls.OnAbility1Use -= () => _abilities[0].TriggerAbility();
        _playerControls.OnAbility2Use -= () => _abilities[1].TriggerAbility();
        _playerControls.OnAbility3Use -= () => _abilities[2].TriggerAbility();
        _meatCollector.OnFullCharge -= () => _canUlt = true;
        _health.OnDie -= Die;
    }

    private void Start()
    {
        /*Application.targetFrameRate = 30;*/
        CanControlPlayer = true;
    }

    private void Update()
    {
        _timeSinceLastJumpPressed += Time.deltaTime;
        _timeSinceLeftGround += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        if (!_canControlPlayer) return;
        CheckCollisions();
        HandleJump();
        HandleMovement();
        HandleGravity();
        ApplyMovement();
    }

    private void CheckCollisions()
    {
        bool groundHit = Physics2D.CircleCast(transform.position + bottomOfsset, rad, Vector2.down, groundDistance, groundLayers);
        bool ceilingHit = Physics2D.CircleCast(transform.position + topOffset, rad, Vector2.up, groundDistance, groundLayers);

        if (ceilingHit) _velocity.y = Mathf.Min(0, _velocity.y);

        // Landed on the Ground
        if (!_isGrounded && groundHit)
        {
            _isGrounded = true;
            _endedJumpEarly = false;
            _canUseCoyote = true;
            _jumpsLeft = maxJumpCount;
            OnGroundChange?.Invoke(true);
        }
        // Left the Ground
        else if (_isGrounded && !groundHit)
        {
            _isGrounded = false;
            _timeSinceLeftGround = 0;
            OnGroundChange?.Invoke(false);
        }

    }

    private void HandleJump()
    {
        if (!_isGrounded && _rb.velocity.y > 0 && !_playerControls.JumpHeld) _endedJumpEarly = true;


        if (_playerControls.JumpDown)
        {
            //bufferd jump data
            _hasBufferdJump = true;
            _timeSinceLastJumpPressed = 0;

            /*            if (_isGrounded || CanUseCoyote())
                        {
                            //first Jump
                            ExecuteJump();
                            return;
                        }*/
            if (_jumpsLeft > 0)
            {
                //MidAirJump
                ExecuteJump();
                return;
            }
        }

        // if the jumps == 0 but the player press jump before landing on ground

        if (HasBufferedJump())
        {
            ExecuteJump();
        }
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _canUseCoyote = false;
        _playerControls.JumpDown = false;
        _hasBufferdJump = false;
        _jumpsLeft--;

        _velocity.y = jumpPower;
        OnPlayerJump?.Invoke();
    }

    private void HandleMovement()
    {
        float horizontalMovement = _playerControls.HorizontalMovement;

        if (horizontalMovement < 0)
        {
            _lastPlayerDirection = -1;
        }
        else if (horizontalMovement > 0)
        {
            _lastPlayerDirection = 1;
        }

        if (horizontalMovement == 0)
        {
            var deceleration = _isGrounded ? groundDeceleration : airDeceleration;
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, horizontalMovement * maxSpeed, moveAcceleration * Time.fixedDeltaTime);
        }
    }

    private void HandleGravity()
    {
        if (_isGrounded && _velocity.y <= 0f)
        {
            _velocity.y = groundingForce;
        }
        else
        {
            float _fallAcceleration = (_endedJumpEarly) ? fallAcceleration * jumpEndEarlyGravityModifier : fallAcceleration;
            _velocity.y = Mathf.MoveTowards(_velocity.y, -maxFallSpeed, _fallAcceleration * Time.fixedDeltaTime);
        }
    }

    private void ApplyMovement()
    {
        if (_platform != null)
        {
            _rb.velocity = _velocity + _platform.GetCurrentSpeed();
        }
        else
        {
            _rb.velocity = _velocity;
        }
    }

    private bool CanUseCoyote()
    {
        return !_isGrounded && _canUseCoyote && _timeSinceLeftGround <= coyoteTime;
    }

    private bool HasBufferedJump()
    {
        return _hasBufferdJump && _isGrounded && _timeSinceLastJumpPressed <= jumpBufferTime;
    }

    public bool IsPlayerFacingLeft()
    {
        return _lastPlayerDirection == -1 ? true : false;
    }

    public float GetPlayerInputVelocity()
    {
        return CanControlPlayer ? _playerControls.HorizontalMovement : 0;
    }

    public void AddVelocity(Vector2 vel)
    {
        _velocity += vel;
    }

    public void SetPlatForm(MovingPlatform transform)
    {
        _platform = transform;
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public bool CanUseUlt()
    {
        return _canUlt;
    }
    public void ResetUltimate()
    {
        _meatCollector.ResetCount();
        _canUlt = false;
    }

    private void Die()
    {
        CanControlPlayer = false;
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        //play animation 
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position + bottomOfsset, rad);
        Gizmos.DrawSphere(transform.position + topOffset, rad);
    }
}
