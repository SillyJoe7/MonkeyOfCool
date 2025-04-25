using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SkateboardBehaviour : MonoBehaviour
{
    [Tooltip("A float value determining the max speed this car can go forwards or backwards.")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rotateAngle = 10f;
    [Tooltip("A float value determining how fast the car accelerates.")]
    [SerializeField] private float acceleration = 10f;
    [Tooltip("A float value determining how fast the car decelerates.")]
    [SerializeField] private float deceleration = 10f;
    [Tooltip("A float value determining how fast the car can turn to the left or right.")]
    [SerializeField] private float turnRate = 180f;
    [Tooltip("A boolean value determining whether or not the car can rotate when not moving.")]
    [SerializeField] private bool onlyTurnWhileMoving = true;
    [SerializeField] private float jumpForce = 100f;
    [Tooltip("The speed at which the skateboard rotates up and down.")]
    [SerializeField] private float rotationSpeed = 5f;  // Adjust this in the inspector for smooth up/down rotation

    // Child object reference
    [SerializeField] private Transform childObject; // Drag the child object in the inspector

    [Header("Air Points Settings")]
    [Tooltip("Rate at which points are added while in the air.")]
    [SerializeField] private float pointRate = 10f; // Exposed point rate

    private float speed, turnInput, moveInput;
    private bool isAirborne = false; // Check if airborne
    private float airTime = 0f; // Track air-time for points
    private Rigidbody rb;
    private Points_SCR points_SCR;

    [HideInInspector] public int Grounded = 0; // Expose Grounded status to see in Inspector

    public float turnAmount => turnInput * turnRate;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        points_SCR = GetComponent<Points_SCR>();

        // Ensure child object is set (can be assigned in inspector)
        if (childObject == null)
        {
            Debug.LogWarning("Child object not assigned in inspector!");
        }
    }

    void Update()
    {
        // Movement logic
        float targetSpeed;
        turnInput = Input.GetAxisRaw("Horizontal");
        moveInput = Input.GetAxisRaw("Vertical");
        targetSpeed = moveInput * maxSpeed;

        if (targetSpeed > speed)
            speed = Mathf.MoveTowards(speed, targetSpeed, acceleration * Time.deltaTime);
        else
            speed = Mathf.MoveTowards(speed, targetSpeed, deceleration * Time.deltaTime);

        if (onlyTurnWhileMoving && Mathf.Abs(speed) == 0.0f)
            turnInput = 0.0f;

        transform.Rotate(Vector3.up * turnAmount * Time.deltaTime, Space.World);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && !isAirborne)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);  // Jump
            isAirborne = true;  // Mark as airborne
        }

        // If airborne, accumulate air-time and add points
        if (isAirborne)
        {
            airTime += Time.deltaTime;
            points_SCR.TrickAddPoints(Mathf.FloorToInt(airTime * pointRate)); // Add points based on time in air
        }
    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;

        // Check if player is grounded
        if (velocity.y <= 0.1f && IsGrounded()) // If player is falling or about to land
        {
            if (isAirborne)
            {
                isAirborne = false;  // Player has landed
                airTime = 0f;  // Reset air-time
            }
        }
    }

    // Helper method to check if the player is grounded
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {
            rb.AddForce(transform.forward * jumpForce / 10, ForceMode.Impulse);
        }
    }
}
