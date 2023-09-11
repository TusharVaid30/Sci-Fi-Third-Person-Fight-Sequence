using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransformHolder;
    [SerializeField] private Transform walkingCamFollow;
    [SerializeField] private Transform runningCamFollow;
    [SerializeField] private float rotationSpeed = 3.0f;
    [SerializeField] private float minVerticalAngle = -80.0f;
    [SerializeField] private float maxVerticalAngle = 80.0f;

    private float _mouseX;
    private float _mouseY;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        cameraTransformHolder.position = transform.position;
        
        _mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        _mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        _mouseY = Mathf.Clamp(_mouseY, minVerticalAngle, maxVerticalAngle);

        cameraTransformHolder.rotation = Quaternion.Euler(new Vector3(0f, _mouseX, 0f));
        walkingCamFollow.localRotation = Quaternion.Euler(new Vector3(_mouseY, 0f, 0f));
        runningCamFollow.localRotation = Quaternion.Euler(new Vector3(_mouseY, 0f, 0f));

        if (_playerMovement.moving)
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraTransformHolder.rotation, 10f * Time.deltaTime);
    }
}