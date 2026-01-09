using UnityEngine;

/// <summary>
/// Smoothly follows a target transform with a configurable offset and speed.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate and smooth movement
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}