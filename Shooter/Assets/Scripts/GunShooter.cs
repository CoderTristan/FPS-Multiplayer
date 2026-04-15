using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooter : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Settings")]
    public float bulletSpeed = 40f;
    public float fireCooldown = 0.15f;

    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    private void OnAttack()
    {
        if (cooldownTimer > 0f)
            return;

        cooldownTimer = fireCooldown;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = firePoint.forward * bulletSpeed;
    }
}
