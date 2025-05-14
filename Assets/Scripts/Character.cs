using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private HealthBarSystem _healthSystem;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 15f;

    private InputAsset _input;
    private Transform _respawnPoint;
    private bool _isGrounded;
    private Vector2 _moveInput;

    private void Awake()
    {
        _healthSystem = GetComponentInChildren<HealthBarSystem>();
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                Debug.LogError("Rigidbody2D component not found on the character.");
            }
        }

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator component not found on the character.");
            }
        }

        if (_sprite == null)
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            if (_sprite == null)
            {
                Debug.LogError("SpriteRenderer component not found on the character.");
            }
        }
    }

    public void Initialize(Transform respawnPoint)
    {
        _respawnPoint = respawnPoint;
        SetupInput();
    }

    private void SetupInput()
    {
        _input = new InputAsset();
        _input.Enable();
        _input.Gameplay.Jump.performed += OnJump;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            State = CharState.Jump;
        }
    }

    public void Respawn()
    {
        if (_respawnPoint != null)
        {
            transform.position = _respawnPoint.position;
            _rigidbody.linearVelocity = Vector2.zero;
            _healthSystem.Heal(_healthSystem.MaxHealth);
        }
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Disable();
            _input.Gameplay.Jump.performed -= OnJump;
        }
    }

    private void FixedUpdate()
    {
        CheckGround();

        // Чтение ввода для движения
        _moveInput = _input.Gameplay.Move.ReadValue<Vector2>();
        Vector2 velocity = new(_moveInput.x * _moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = velocity;

        
        if (_moveInput.x != 0)
            _sprite.flipX = _moveInput.x < 0.0f;

        // Управление анимацией
        if (_isGrounded)
        {
            if (Mathf.Abs(_moveInput.x) > Mathf.Epsilon)
                State = CharState.Move;
            else
                State = CharState.Idle;
        }
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        _isGrounded = colliders.Length > 1;
        if (!_isGrounded) State = CharState.Jump;
    }

    private CharState State
    {
        get => (CharState)_animator.GetInteger("State");
        set => _animator.SetInteger("State", (int)value);
    }

    public enum CharState
    {
        Idle,
        Move,
        Jump
    }
}
