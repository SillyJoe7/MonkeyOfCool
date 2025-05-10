using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 300f; // Speed of rotation
    public float leanSpeed = 100f; // Speed of leaning left and right
    public float jumpForce = 5f; // Jump force applied
    public string groundTag = "Ground"; // Ground tag
    public float rotationThreshold = 30f; // Minimum rotation to count for points

    private bool isTouchingGround = false; // To check if the object is grounded
    private bool wasInAir = false; // To check if the object was in the air during the rotation
    private float rotationAccumulated = 0f; // Total accumulated rotation in degrees

    private Rigidbody rb;
    private Points_SCR pointsManager; // Reference to the Points_SCR component

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("Rigidbody required for jumping.");

        GameObject pointsObject = GameObject.FindWithTag("Player");
        if (pointsObject != null)
            pointsManager = pointsObject.GetComponent<Points_SCR>();

        if (pointsManager == null)
            Debug.LogWarning("Points_SCR not found. Make sure the object has the tag 'Player'.");
    }

    void Update()
    {
        // Jump logic when the object is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isTouchingGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isTouchingGround = false;
        }

        // Rotate object in the air based on mouse movement
        if (!isTouchingGround && Input.GetMouseButton(0))
        {
            float mouseY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f);
            float mouseX = Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f);

            float flipRotation = -mouseY * rotationSpeed * Time.deltaTime;
            float leanRotation = -mouseX * leanSpeed * Time.deltaTime;

            transform.Rotate(Vector3.right, flipRotation);
            transform.Rotate(Vector3.forward, leanRotation);

            wasInAir = true;
            rotationAccumulated += Mathf.Abs(flipRotation) + Mathf.Abs(leanRotation);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            // When landing, calculate points for rotation
            if (wasInAir && !isTouchingGround)
            {
                if (rotationAccumulated >= rotationThreshold) // Only add points if rotation exceeds threshold
                {
                    int pointsToAdd = Mathf.RoundToInt(rotationAccumulated);
                    Debug.Log($"Rotated in air: {rotationAccumulated} degrees. Adding {pointsToAdd} points.");
                    if (pointsManager != null)
                    {
                        pointsManager.TrickAddPoints(pointsToAdd);
                    }
                }

                rotationAccumulated = 0f; // Reset rotation accumulation
                wasInAir = false;
            }

            isTouchingGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            isTouchingGround = false;
        }
    }
}
