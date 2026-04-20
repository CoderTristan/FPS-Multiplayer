using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.LookAt(target);
            transform.Rotate(0, 180f, 0);
        }
    }
}
