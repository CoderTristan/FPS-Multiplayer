using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // First check tag
        if (!other.CompareTag("Player"))
        {
            print("Non-player object entered kill zone, ignoring...");
            return;
        }

        // Then try to get the PlayerControl script
        PlayerControl pc = other.GetComponent<PlayerControl>();
        if (pc != null)
        {
            print("Player entered kill zone, respawning...");
            pc.Die();
        }
    }
}
