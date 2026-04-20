using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 6f;
    public float gravity = -40f;
    public float jumpForce = 10f;

    [Header("Respawn")]
    public Transform respawnPoint;
    public static int deathCount = 0;


    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private bool jumpQueued = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 forwardMove = transform.forward * forwardSpeed;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (isGrounded && jumpQueued)
        {
            velocity.y = jumpForce;
            jumpQueued = false;
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = forwardMove + new Vector3(0, velocity.y, 0);
        controller.Move(finalMove * Time.deltaTime);
    }

    private void OnJump()
    {
        jumpQueued = true;
    }

    public void Die()
{
    deathCount++;

    controller.enabled = false;
    transform.position = respawnPoint.position;
    transform.rotation = respawnPoint.rotation;
    controller.enabled = true;

    velocity = Vector3.zero;

    SkyDeathCounter.instance.UpdateText();
}

}
