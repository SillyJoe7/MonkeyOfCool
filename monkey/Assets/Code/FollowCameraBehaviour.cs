using UnityEngine;

public class FollowCameraBehaviour : MonoBehaviour
{
    [Tooltip("A GameObject reference value determining which GameObject the Camera will follow")]
    [SerializeField] private GameObject target;
    [SerializeField] private float sensitivity = 2f; // Mouse sensitivity for rotation
    [SerializeField] private float scrollSpeed = 2f; // Speed of zooming in/out
    [SerializeField] private float minDistance = 2f; // Minimum zoom distance
    [SerializeField] private float maxDistance = 10f; // Maximum zoom distance

    private Vector3 offset;
    private float pitch = 0f; // Vertical angle
    private float yaw = 0f;   // Horizontal angle
    private float distance;   // Current zoom distance

    void Start()
    {
        if (target)
        {
            offset = transform.position - target.transform.position;
            distance = offset.magnitude; // Initial zoom distance
            yaw = transform.eulerAngles.y;
            pitch = transform.eulerAngles.x;
        }
    }

    void LateUpdate()
    {
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
        }
    }
}
