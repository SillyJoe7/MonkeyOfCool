using UnityEngine;

public class SmoothTimedRotation : MonoBehaviour
{
    [Tooltip("How many degrees to rotate.")]
    public float rotationAmount = 360f;

    [Tooltip("How long (in seconds) to complete the rotation.")]
    public float duration = 2f;

    [Tooltip("Which axis to rotate around (set in the Inspector).")]
    public Vector3 rotationAxis = Vector3.up;  // You can set this in Unity Inspector

    private float elapsed = 0f;
    private float totalRotation = 0f;
    private bool rotating = false;
    Points_SCR points_SCR;

    void Start() {
        points_SCR = GetComponent<Points_SCR>();
    }

    void Update()
    {
        // Check if "R" key is pressed to start rotation
        if (Input.GetKeyDown(KeyCode.R) && !rotating)
        {
            StartRotation();
        }

        // If we are rotating, update the rotation incrementally
        if (rotating)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Calculate how much rotation to apply this frame
            float rotationThisFrame = rotationAmount * t - totalRotation;

            // Apply the rotation to the object
            transform.Rotate(rotationAxis, rotationThisFrame);

            // Keep track of the total rotation applied so far
            totalRotation += rotationThisFrame;

            if (totalRotation >= rotationAmount)
            {
                rotating = false; // Stop the rotation once we reach the target
            }
        }
    }

    // Function to start the rotation
    private void StartRotation()
    {
        elapsed = 0f; // Reset elapsed time
        totalRotation = 0f; // Reset total rotation applied
        rotating = true; // Begin the rotation
        points_SCR.TrickAddPoints(7);
    }
}
