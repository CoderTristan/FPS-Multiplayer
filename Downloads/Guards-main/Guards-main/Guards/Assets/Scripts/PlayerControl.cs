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

    [Header("Interaction")]
    public float interactRange = 3f;

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
        float g = Physics.gravity.y * gravityMultiplier;

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

            if (jumpRequested)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * g);
                jumpRequested = false;
            }
        }
        else
        {
            velocity.y += g * Time.deltaTime;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 finalMove = move * speed + new Vector3(0, velocity.y, 0);
        controller.Move(finalMove * Time.deltaTime);
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

    private void OnAttack()
    {
        Ray ray = new Ray(cameraTarget.position, cameraTarget.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("PickUp"))
            {
                // Component check
                if (hit.collider.TryGetComponent(out PickUp pickup))
                {
                    pickup.PickUpItem();
                }
            }
        }
    }
}
