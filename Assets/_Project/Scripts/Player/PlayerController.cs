using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float airControl = 0.45f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float groundCheckDistance = 0.8f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Camera mainCamera;
    private bool isGrounded;

    private Vector3 moveInput;
    private Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        rb.freezeRotation = true;
    }

    private void Update()
    {
        ReadMovementInput();
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(horizontal, 0f, vertical).normalized;

        if (mainCamera == null)
        {
            moveDirection = moveInput;
            return;
        }

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraForward * moveInput.z + cameraRight * moveInput.x).normalized;
    }

    private void Move()
    {
        float controlMultiplier = isGrounded ? 1f : airControl;

        Vector3 targetVelocity = moveDirection * moveSpeed * controlMultiplier;

        Vector3 currentVelocity = rb.velocity;

        Vector3 velocityChange = new Vector3(
            targetVelocity.x - currentVelocity.x,
            0f,
            targetVelocity.z - currentVelocity.z
        );

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void Jump()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0f;
        rb.velocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        isGrounded = Physics.Raycast(
            rayStart,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }
}