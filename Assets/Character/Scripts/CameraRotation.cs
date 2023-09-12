using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransformHolder;
    [SerializeField] private Transform walkingCamFollow;
    [SerializeField] private Transform runningCamFollow;
    [SerializeField] private float rotationSpeed = 3.0f;
    [SerializeField] private float minVerticalAngle = -80.0f;
    [SerializeField] private float maxVerticalAngle = 80.0f;
    [SerializeField] private Transform spineRotationTarget;
    [SerializeField] private float spineBendFactor;
    [SerializeField] private Transform headRotationTarget;
    [SerializeField] private float headRotationFactor;
    
    private float _mouseX;
    private float _mouseY;
    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;
    private InputAction _mouseMovement;
    private float _headX = 0f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _mouseMovement = _playerInput.actions["Camera Rotation"];
    }

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        cameraTransformHolder.position = transform.position;

        Vector2 mouseAxis = _mouseMovement.ReadValue<Vector2>();

        _mouseX += mouseAxis.x * rotationSpeed;
        _mouseY -= mouseAxis.y * rotationSpeed;
        _mouseY = Mathf.Clamp(_mouseY, minVerticalAngle, maxVerticalAngle);
        _headX += mouseAxis.x / headRotationFactor;
        _headX = Mathf.Clamp(_headX, -1f, 1f);

        cameraTransformHolder.rotation = Quaternion.Euler(new Vector3(0f, _mouseX, 0f));
        walkingCamFollow.localRotation = Quaternion.Euler(new Vector3(_mouseY, 0f, 0f));
        runningCamFollow.localRotation = Quaternion.Euler(new Vector3(_mouseY, 0f, 0f));

        if (_playerMovement.moving)
        {
            transform.rotation =
                Quaternion.Lerp(transform.rotation, cameraTransformHolder.rotation, 10f * Time.deltaTime);

            Quaternion rot = Quaternion.Euler(0f, 0f, -mouseAxis.x / spineBendFactor);
            spineRotationTarget.localRotation =
                Quaternion.Lerp(spineRotationTarget.localRotation, rot, 10f * Time.deltaTime);
            headRotationTarget.localRotation = Quaternion.Lerp(headRotationTarget.localRotation, Quaternion.identity,
                3f * Time.deltaTime);
        }
        else
        {
            spineRotationTarget.localRotation = Quaternion.Lerp(spineRotationTarget.localRotation, Quaternion.identity,
                10f * Time.deltaTime);
        }

        if (!_playerMovement.runningFast)
        {
            var headLocalPos = headRotationTarget.localPosition;
            headLocalPos.x = Mathf.Lerp(headLocalPos.x, _headX, 3f * Time.deltaTime);
            headRotationTarget.localPosition = headLocalPos;
        }
        else if (mouseAxis.x == 0f)
        {
            var headLocalPos = headRotationTarget.localPosition;
            headLocalPos.x = Mathf.Lerp(headLocalPos.x, 0f, 3f * Time.deltaTime);
            headRotationTarget.localPosition = headLocalPos;
        }
    }
}