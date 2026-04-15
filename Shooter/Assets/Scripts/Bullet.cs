using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
