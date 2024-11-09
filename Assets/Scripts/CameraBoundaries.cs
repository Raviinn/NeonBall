using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    public Transform target;        // The target the camera follows, usually the player

    [Header("Camera Boundaries")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    private void LateUpdate()
    {
        if (target != null)
        {
            // Get the target's position
            Vector3 targetPosition = target.position;

            // Clamp the camera position within the boundaries
            float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);

            // Set the camera position, keeping the z-position unchanged
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
