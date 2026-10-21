
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float sprintSpeed = 12f;
    public float rotationSpeed = 12f;

    [Header("Jump and Gravity")]
    public float jumpHeight = 2.5f;
    public float gravity = -25f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 dashDirection;
    private float dashTimer;
    private float dashCooldownTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();
        ApplyGravity();
    }

    void HandleMovement()
    {
        if (dashTimer > 0f)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);
        input = Vector3.ClampMagnitude(input, 1f);

        // Movement relative to the player's facing direction.
        Vector3 moveDirection = transform.TransformDirection(input);

        bool sprinting =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        float currentSpeed = sprinting ? sprintSpeed : moveSpeed;

        controller.Move(
            moveDirection * currentSpeed * Time.deltaTime
        );

        // Face the direction of movement.
        if (input.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleJump()
    {
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (controller.isGrounded &&
            Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleDash()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (dashTimer > 0f)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;
            return;
        }

        // Press Q to dash in your movement direction.
        if (Input.GetKeyDown(KeyCode.Q) &&
            dashCooldownTimer <= 0f)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 input = new Vector3(horizontal, 0f, vertical);
            input = Vector3.ClampMagnitude(input, 1f);

            dashDirection = input.sqrMagnitude > 0.01f
                ? transform.TransformDirection(input).normalized
                : transform.forward;

            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(
            Vector3.up * velocity.y * Time.deltaTime
        );
    }
}
