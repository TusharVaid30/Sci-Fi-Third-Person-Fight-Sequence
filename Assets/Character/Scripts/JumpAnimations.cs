using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class JumpAnimations : MonoBehaviour
{
    public InputAction jumpAction;
    
    [SerializeField] private Animator playerAnim;
    [SerializeField] private MultiAimConstraint headAimConstraint;
    
    private PlayerInput _playerInput;
    private static readonly int JumpStart = Animator.StringToHash("Jump Start");
    private static readonly int JumpInAir = Animator.StringToHash("Jump In Air");
    private static readonly int JumpEnd = Animator.StringToHash("Jump End");

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        jumpAction = _playerInput.actions["Jump"];
        jumpAction.performed += JumpStartTrigger;
        jumpAction.canceled += JumpInAirTrigger;
    }

    private void JumpStartTrigger(InputAction.CallbackContext obj)
    {
        playerAnim.SetTrigger(JumpStart);
        headAimConstraint.weight = 0f;
        playerAnim.ResetTrigger(JumpEnd);
    }

    private void JumpInAirTrigger(InputAction.CallbackContext obj)
    {
        playerAnim.SetTrigger(JumpInAir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerAnim.SetTrigger(JumpEnd);
    }
}
