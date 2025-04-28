using UnityEngine;

public class FollowCameraBehaviour : MonoBehaviour
{
    [Tooltip("A GameObject reference value determining which GameObject the Camera will follow")]
    [SerializeField] private GameObject target;
    [SerializeField] private float sensitivity = 2f; // Mouse sensitivity for rotation
    [SerializeField] private float scrollSpeed = 2f; // Speed of zooming in/out
    [SerializeField] private float minDistance = 2f; // Minimum zoom distance
    [SerializeField] private float maxDistance = 10f; // Maximum zoom distance

    private Vector3 dynamicOffset;
    private float pitch = 0f; // Vertical angle
    private float yaw = 0f;   // Horizontal angle
    private float distance;   // Current zoom distance
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float followDistance = 5f;
    [SerializeField] private float height = 2f;
    public float directionSmoothSpeed = 5f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private LayerMask collisionLayers;



private Vector3 currentFollowDirection;

    void Start()
    {
        if (target)
        {
            dynamicOffset = -target.transform.forward * followDistance + Vector3.up * height;
            distance = dynamicOffset.magnitude; // Initial zoom distance
            yaw = transform.eulerAngles.y;
            pitch = transform.eulerAngles.x;
        }
    }
// work in progress
    void LateUpdate() {
        // Smooth the follow direction behind the target
        Vector3 desiredDirection = -target.transform.forward;
        currentFollowDirection = Vector3.Slerp(currentFollowDirection, desiredDirection, Time.deltaTime * directionSmoothSpeed);

        // Desired camera position before any collision
        Vector3 desiredOffset = currentFollowDirection * followDistance + Vector3.up * height;
        Vector3 desiredCameraPos = target.transform.position + desiredOffset;

        // Cast from target to desired camera position
        Ray ray = new Ray(target.transform.position + Vector3.up * 1.5f, desiredOffset.normalized);
        float distance = desiredOffset.magnitude;

        if (Physics.Raycast(ray, out RaycastHit hit, distance, collisionLayers)) {
            // Obstacle in the way: move camera to the hit point (a little closer to avoid clipping)
            desiredCameraPos = hit.point - ray.direction * 0.3f; // small buffer
        }

        // Smooth camera position
        transform.position = Vector3.SmoothDamp(transform.position, desiredCameraPos, ref velocity, smoothTime);

        // Look at character
        transform.LookAt(target.transform.position + Vector3.up * 1.5f);
    }
/*
// my work
    void LateUpdate() {
        if (target) {
            // Smooth the follow direction (behind character)
            Vector3 desiredDirection = -target.transform.forward;
            currentFollowDirection = Vector3.Slerp(currentFollowDirection, desiredDirection, Time.deltaTime * directionSmoothSpeed);

            // Compute dynamic offset from smoothed direction
            Vector3 offset = currentFollowDirection * followDistance + Vector3.up * height;
            Vector3 targetPosition = target.transform.position + offset;

            // Smooth the camera's position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // Look at the target (e.g. character’s upper body or head)
            transform.LookAt(target.transform.position + Vector3.up * 1.5f);
        }

        /*
        // old player movement camera
        if (target)
        {
            // Handle camera rotation when right mouse button is held
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

                yaw += mouseX;
                pitch -= mouseY;
                pitch = Mathf.Clamp(pitch, -90f, 90f); // Prevents looking too far up or down
            }

            // Handle zooming with mouse scroll wheel
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            distance -= scrollInput * scrollSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance); // Limits zoom range

            // Apply rotation and zoom
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 newOffset = rotation * Vector3.back * distance; // Moves camera back based on zoom

            transform.position = target.transform.position + newOffset;
            transform.LookAt(target.transform.position);
        } */
    //}
}
