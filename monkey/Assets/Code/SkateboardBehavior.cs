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
    Points_SCR points_SCR;

    // New variable for controlling the up/down rotation speed
    [Tooltip("The speed at which the skateboard rotates up and down.")]
    [SerializeField] private float rotationSpeed = 5f;  // Adjust this in the inspector for smooth up/down rotation

    // Child object reference
    [SerializeField] private Transform childObject; // Drag the child object in the inspector

    private float speed, turnInput, moveInput;
    public int Grounded = 0;
    private Rigidbody rb;

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

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && Grounded == 0)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Grounded = 1;
            points_SCR.TrickAddPoints();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Grounded == 1)
        {
            rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);
            Grounded = 2;
            points_SCR.TrickAddPoints();
        }

    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        Vector3 currentEuler = transform.rotation.eulerAngles;
        float targetPitch = 0f;

        if (Grounded == 1 || Grounded == 2)
        {
            if (velocity.sqrMagnitude > 0.01f)
            {
                float flatSpeed = new Vector2(velocity.x, velocity.z).magnitude;
                targetPitch = Mathf.Atan2(velocity.y, flatSpeed) * Mathf.Rad2Deg;
            }
        }

        // Smoothly interpolate pitch toward targetPitch using rotationSpeed
        float currentPitch = NormalizeAngle(currentEuler.x);
        float smoothPitch = Mathf.LerpAngle(currentPitch, -targetPitch, Time.fixedDeltaTime * rotationSpeed); // Use rotationSpeed for smoothness

        transform.rotation = Quaternion.Euler(smoothPitch, currentEuler.y, 0f);

        // If child object exists, make sure it follows the parent’s position/rotation
        if (childObject != null)
        {
            childObject.position = transform.position; // Sync position with parent
            childObject.rotation = transform.rotation; // Sync rotation with parent
        }
    }

    // Helper to ensure angle behaves correctly around 360° wraparound
    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {
            rb.AddForce(transform.forward * jumpForce / 10, ForceMode.Impulse);
        }
    }
}
