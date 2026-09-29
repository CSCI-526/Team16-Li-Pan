using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Follow")]
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Bounds")]
    [SerializeField] private BoxCollider2D cameraBounds;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player == null || cameraBounds == null)
            return;

        // Camera half-size
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // Read map boundaries directly from the collider
        Bounds bounds = cameraBounds.bounds;

        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minY = bounds.min.y;
        float maxY = bounds.max.y;

        // Follow player
        Vector3 targetPosition = player.position;

        // Keep the entire camera view inside the map
        targetPosition.x = Mathf.Clamp(
            targetPosition.x,
            minX + halfWidth,
            maxX - halfWidth
        );

        targetPosition.y = Mathf.Clamp(
            targetPosition.y,
            minY + halfHeight,
            maxY - halfHeight
        );

        targetPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}