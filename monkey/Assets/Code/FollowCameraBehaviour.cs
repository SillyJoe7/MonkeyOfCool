using UnityEngine;

public class FollowCameraBehaviour : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject target;

    [Header("Camera Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask collisionLayers;

    [Header("Trick Control")]
    public bool isPerformingTrick = false; // Set true/false from your trick code

    private float pitch = 20f;
    private float yaw = 0f;
    private float distance = 5f;

    void Start()
    {
        if (target)
        {
            yaw = transform.eulerAngles.y;
            pitch = transform.eulerAngles.x;
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        HandleInput();

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Position directly relative to the target
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 desiredPosition = target.transform.position + offset;

        // Collision check (optional: you can remove this if you don't even want collision correction)
        Vector3 targetCenter = target.transform.position + Vector3.up * 1.5f;
        Vector3 direction = (desiredPosition - targetCenter).normalized;
        float desiredDistance = Vector3.Distance(targetCenter, desiredPosition);

        if (Physics.SphereCast(targetCenter, 0.3f, direction, out RaycastHit hit, desiredDistance, collisionLayers))
        {
            desiredPosition = targetCenter + direction * (hit.distance - 0.3f);
        }

        transform.position = desiredPosition;
        transform.LookAt(targetCenter);
    }

    private void HandleInput()
    {
        if (isPerformingTrick)
            return; // Ignore mouse input during tricks

        if (Input.GetMouseButton(1)) // Right mouse button held
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -45f, 80f);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollInput * scrollSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }
}
