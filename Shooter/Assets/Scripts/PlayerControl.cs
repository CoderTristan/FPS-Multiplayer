using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTarget;

    [Header("Movement")]
    public float walkSpeed = 8f;
    public float runSpeed = 12f;
    public float jumpHeight = 2f;
    public float gravityMultiplier = 2f;

    [Header("Look")]
    public float lookSensitivity = 20f;
    public float minPitch = -80f;
    public float maxPitch = 80f;
    public bool invertY = false;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float cameraPitch;
    private bool isRunning;
    private bool jumpRequested;

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        if (controller.isGrounded)
        {
            velocity.y = -2f;

            if (jumpRequested)
            {
                float g = Physics.gravity.y * gravityMultiplier;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * g);
                jumpRequested = false;
            }
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y = 0f;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        float dt = Time.deltaTime;

        float yaw = lookInput.x * lookSensitivity * dt;
        float pitch = lookInput.y * lookSensitivity * dt;

        if (invertY) pitch = -pitch;

        transform.Rotate(Vector3.up * yaw);

        cameraPitch = Mathf.Clamp(cameraPitch - pitch, minPitch, maxPitch);
        cameraTarget.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    // Input System Callbacks
    private void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    private void OnLook(InputValue value) => lookInput = value.Get<Vector2>();
    private void OnRun(InputValue value) => isRunning = value.isPressed;

    private void OnJump()
    {
        if (controller.isGrounded)
            jumpRequested = true;
    }
}
