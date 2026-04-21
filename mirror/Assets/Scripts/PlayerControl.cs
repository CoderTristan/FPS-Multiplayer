using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;

    [Header("Movement")]
    public float forwardSpeed = 6f;
    public float gravity = -50f;
    public float jumpForce = 14f;

    [Header("Respawn")]
    public Transform respawnPoint;
    public static int deathCount = 0;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpQueued = false;

    private bool isFrozen = false; // stops movement during death delay

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // If frozen, skip all movement
        if (isFrozen)
            return;

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

    // Update sky text
    if (SkyDeathCounter.instance != null)
        SkyDeathCounter.instance.UpdateText();

    StartCoroutine(RespawnRoutine());
}


    private IEnumerator RespawnRoutine()
{
    // Freeze player movement
    isFrozen = true;
    velocity = Vector3.zero;

    // Stop music instantly
    if (musicSource != null)
    {
        musicSource.Stop();
        musicSource.time = 0f;
    }

    // Wait 1 second before respawn
    yield return new WaitForSeconds(1f);

    // --- TELEPORT CLEANLY ---
    controller.enabled = false; // reset internal state
    transform.position = respawnPoint.position;
    transform.rotation = respawnPoint.rotation;
    controller.enabled = true;

    // Clear velocity again after enabling controller
    velocity = Vector3.zero;

    // Skip one frame so controller doesn't apply old movement
    yield return null;

    // Restart music
    if (musicSource != null)
        musicSource.Play();

    // Unfreeze player
    isFrozen = false;
}

}
