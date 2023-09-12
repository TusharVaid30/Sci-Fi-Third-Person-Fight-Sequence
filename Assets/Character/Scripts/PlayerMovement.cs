using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool moving;
    public bool runningFast;
    
    [Header("Player Settings")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float maxWalkingSpeed;
    [SerializeField] private float maxRunningSpeed;
    
    [Header("Camera Settings")]
    [SerializeField] private GameObject[] cameras;
    [SerializeField] private MultiAimConstraint headConstraint;

    private PlayerInput _playerInput;
    private InputAction _movementAction;
    private InputAction _runAction;
    
    private static readonly int X = Animator.StringToHash("x");
    private static readonly int Y = Animator.StringToHash("y");

    private int _speedReduction = 2;
    private Vector2 _direction;
    private int _diagonalDir;
    private Transform _playerChild;
    private bool _running;
    private Rigidbody _playerRb;
    private Vector3 _moveDirection;
    private float _velocity;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        
        _movementAction = _playerInput.actions["Movement"];
        _movementAction.performed += Move;
        _movementAction.canceled += EndMove;
        
        _runAction = _playerInput.actions["Run"];
        _runAction.performed += StartRun;
        _runAction.canceled += EndRun;
        
        _direction = Vector2.zero;

        _playerChild = transform.GetChild(0);

        _playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerAnimator.SetFloat(X, _direction.x / _speedReduction, 0.1f, Time.deltaTime);
        playerAnimator.SetFloat(Y, _direction.y / _speedReduction, 0.1f, Time.deltaTime);

        Quaternion orientation = Quaternion.Euler(0f, 45f * Mathf.Ceil(_direction.x * _direction.y), 0f);

        _playerChild.localRotation = Quaternion.Lerp(_playerChild.localRotation, orientation, 10f * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 vel = _playerRb.velocity;
        Vector3 targetVelocity = transform.TransformDirection(_moveDirection) * (_velocity * Time.fixedDeltaTime);
        vel.x = targetVelocity.x;
        vel.z = targetVelocity.z;
        _playerRb.velocity = vel;
    }

    private void Move(InputAction.CallbackContext obj)
    {
        _direction = obj.ReadValue<Vector2>();
        moving = true;
        _moveDirection = new Vector3(_direction.x, 0f, _direction.y);

        if (_direction.y < 0f)
            SetWalkingState();
        
        _velocity = !runningFast ? maxWalkingSpeed : maxRunningSpeed;
    }

    private void EndMove(InputAction.CallbackContext obj)
    {
        _direction = Vector2.zero;
        _velocity = 0f;
        moving = false;
        SetWalkingState();
    }

    private void StartRun(InputAction.CallbackContext obj)
    {
        if (!moving || _direction.y < 0f) return;
        SetRunningState();
        _velocity = maxRunningSpeed;
        headConstraint.weight = 0f;
    }

    private void EndRun(InputAction.CallbackContext obj)
    {
        SetWalkingState();
        _velocity = moving ? maxWalkingSpeed : 0f;
        headConstraint.weight = 1f;
    }

    private void SetWalkingState()
    {
        cameras[0].SetActive(true);
        cameras[1].SetActive(false);
        runningFast = false;
        _speedReduction = 2;
    }
    
    private void SetRunningState()
    {
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
        runningFast = true;
        _speedReduction = 1;
    }
}
