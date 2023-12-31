using UnityEngine;

public class ResponsiveJump : MonoBehaviour
{
    [SerializeField] private JumpController jumpController;
    
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float highJumpMultiplier;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_rb.velocity.y < 0f)
            _rb.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        else if (_rb.velocity.y > 0f && !jumpController.jumping)
            _rb.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        else if (_rb.velocity.y > 0f && jumpController.jumping)
            _rb.velocity += Vector3.up * (Physics.gravity.y * (highJumpMultiplier - 1) * Time.deltaTime);
    }
}