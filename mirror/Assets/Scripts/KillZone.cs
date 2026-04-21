using UnityEngine;

public class KillZone : MonoBehaviour
{
    public AudioSource deathSound; // assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            print("Non-player object entered kill zone, ignoring...");
            return;
        }

        PlayerControl pc = other.GetComponent<PlayerControl>();
        if (pc != null)
        {
            print("Player entered kill zone, respawning...");

            // Play sound BEFORE respawn
            if (deathSound != null)
                deathSound.Play();

            pc.Die();
        }
    }
}
