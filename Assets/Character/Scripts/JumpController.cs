using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class JumpController : MonoBehaviour
{
    public bool jumping;

    [SerializeField] private JumpAnimations jumpAnimations;
    [SerializeField] private float jumpForce;
    
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        jumpAnimations.jumpAction.canceled += Jump;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        jumping = true;
        _rb.velocity = Vector3.up * jumpForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
            jumping = false;
    }
}
