using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool moving;
    
    [Header("Player Settings")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float maxWalkingSpeed;
    [SerializeField] private float maxRunningSpeed;
    
    [Header("Camera Settings")]
    [SerializeField] private GameObject[] cameras;

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
    private bool _runningFast;
    
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
        _playerRb.velocity = transform.TransformDirection(_moveDirection) * (_velocity * Time.fixedDeltaTime);
    }

    private void Move(InputAction.CallbackContext obj)
    {
        _direction = obj.ReadValue<Vector2>();
        moving = true;
        _moveDirection = new Vector3(_direction.x, 0f, _direction.y);
        if (!_runningFast)
            _velocity = maxWalkingSpeed;
        else
            _velocity = maxRunningSpeed;
    }

    private void EndMove(InputAction.CallbackContext obj)
    {
        _direction = Vector2.zero;
        moving = false;
        _velocity = 0f;
    }

    private void StartRun(InputAction.CallbackContext obj)
    {
        if (!moving || _direction.y < 0f) return;
        _speedReduction = 1;
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
        _velocity = maxRunningSpeed;
        _runningFast = true;
    }

    private void EndRun(InputAction.CallbackContext obj)
    {
        _speedReduction = 2;
        cameras[0].SetActive(true);
        cameras[1].SetActive(false);
        _runningFast = false;
        if (moving)
            _velocity = maxWalkingSpeed;
        else
            _velocity = 0f;
    }
}
