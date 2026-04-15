using UnityEngine;

public class SurfaceWalkerSphere : MonoBehaviour
{
    [Header("Surface Detection")]
    public LayerMask walkableMask;
    public float sphereRadius = 0.6f;
    public float probeDistance = 1.2f;

    [Header("Behavior")]
    public float alignSpeed = 10f;
    public float snapStrength = 12f;
    public float gravityStrength = 25f;

    private Vector3 currentUp;

    public Vector3 GravityDirection => -currentUp;

    private void Start()
    {
        currentUp = transform.up;
    }

    private void Update()
    {
        HandleSurface();
    }

    private void HandleSurface()
    {
        if (ProbeSurface(out RaycastHit hit))
        {
            Vector3 targetUp = hit.normal;

            // Smooth normal blending
            currentUp = Vector3.Slerp(currentUp, targetUp, Time.deltaTime * alignSpeed);

            // Rotate player to match new up direction
            Quaternion targetRot =
                Quaternion.FromToRotation(transform.up, currentUp) * transform.rotation;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * alignSpeed
            );

            // Snap to surface to prevent drifting/flying
            Vector3 targetPos = hit.point + hit.normal * (sphereRadius * 0.9f);
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * snapStrength
            );
        }
        else
        {
            transform.position += GravityDirection * gravityStrength * Time.deltaTime;
        }
    }

    private bool ProbeSurface(out RaycastHit bestHit)
    {
        bestHit = new RaycastHit();
        float bestDist = Mathf.Infinity;
        bool found = false;
        
        Vector3 origin = transform.position + currentUp * (sphereRadius * 0.5f);

        // Probe in 6 directions
        Vector3[] dirs = new Vector3[]
        {
            -currentUp,
            currentUp,
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };

        foreach (var dir in dirs)
        {
            if (Physics.SphereCast(origin, sphereRadius, dir, out RaycastHit hit, probeDistance, walkableMask))
            {
                if (hit.distance < bestDist)
                {
                    bestDist = hit.distance;
                    bestHit = hit;
                    found = true;
                }
            }
        }

        return found;
    }

    
}
