using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float rotationSpeed = 10f;

    [Header("Jumping & Gravity")]
    public float jumpHeight = 2.5f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;

    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Animator animator;

    private Vector2 moveInput;
    private bool jumpInput;
    private bool sprintInput;

    private float verticalVelocity;
    private bool isGrounded;

    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleRotation();
        HandleJump();
        ApplyGravity();
        ApplyFinalMovement();
        HandleAnimations();
    }

    // -----------------------------
    // INPUT SYSTEM EVENTS
    // -----------------------------
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jumpInput = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        sprintInput = value.isPressed;
    }

    // -----------------------------
    // MOVEMENT
    // -----------------------------
    private void HandleMovement()
    {
        float speed = sprintInput ? sprintSpeed : walkSpeed;

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.magnitude > 1f)
            move.Normalize();

        Vector3 worldMove = transform.TransformDirection(move) * speed;

        controller.Move(new Vector3(worldMove.x, verticalVelocity, worldMove.z) * Time.deltaTime);
    }

    // -----------------------------
    // ROTATION
    // -----------------------------
    private void HandleRotation()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // -----------------------------
    // GROUND CHECK
    // -----------------------------
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f; // keeps player grounded
    }

    // -----------------------------
    // JUMPING
    // -----------------------------
    private void HandleJump()
    {
        if (jumpInput && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // -----------------------------
    // GRAVITY
    // -----------------------------
    private void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }

    // -----------------------------
    // FINAL MOVEMENT
    // -----------------------------
    private void ApplyFinalMovement()
    {
        // Movement already applied inside HandleMovement
    }

    // -----------------------------
    // ANIMATIONS
    // -----------------------------
    private void HandleAnimations()
    {
        if (animator == null) return;

        float speedPercent = moveInput.magnitude;

        animator.SetFloat("Speed", speedPercent);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", verticalVelocity);
    }
}
