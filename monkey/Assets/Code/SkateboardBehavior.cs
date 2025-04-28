using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SkateboardBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float turnRate = 180f;
    [SerializeField] private bool onlyTurnWhileMoving = true;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 100f;

    [Header("Visual Tilt")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform childObject;

    [Header("Air Points Settings")]
    [SerializeField] private float pointRate = 10f;

    [Header("Drift Settings")]
    [SerializeField] private float driftTurnMultiplier = 2f;
    [SerializeField] private float driftFrictionMultiplier = 0.5f;

    private float currentSpeed = 0f;
    private float speedVelocity = 0f;
    private float turnInput, moveInput;
    private bool isDrifting = false;

    public float turnAmount => turnInput * turnRate;

    private bool isAirborne = false;
    private float airTime = 0f;

    private Rigidbody rb;
    private Points_SCR points_SCR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        points_SCR = GetComponent<Points_SCR>();

        if (childObject == null)
            Debug.LogWarning("Child object not assigned in inspector!");
    }

    void Update()
    {
        // Read input
        turnInput = Input.GetAxis("Horizontal");
        moveInput = Input.GetAxis("Vertical");
        isDrifting = Input.GetKey(KeyCode.LeftShift);

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && !isAirborne)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isAirborne = true;
        }

        // Track air time
        if (isAirborne)
        {
            airTime += Time.deltaTime;
            points_SCR.TrickAddPoints(Mathf.FloorToInt(airTime * pointRate));
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleGroundCheck();
    }

    private void HandleMovement()
    {
        float targetSpeed = moveInput * maxSpeed;

        float smoothTime = (Mathf.Abs(targetSpeed) > Mathf.Abs(currentSpeed)) ? (1f / acceleration) : (1f / deceleration);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothTime);

        if (!onlyTurnWhileMoving || Mathf.Abs(currentSpeed) > 0.1f)
        {
            float driftMultiplier = isDrifting ? driftTurnMultiplier : 1f;
            float rotationAmount = turnInput * turnRate * driftMultiplier * Time.fixedDeltaTime;
            Quaternion turnOffset = Quaternion.Euler(0f, rotationAmount, 0f);
            rb.MoveRotation(rb.rotation * turnOffset);
        }

        float frictionMultiplier = isDrifting ? driftFrictionMultiplier : 1f;
        Vector3 forwardMove = transform.forward * currentSpeed * Time.fixedDeltaTime * frictionMultiplier;
        rb.MovePosition(rb.position + forwardMove);

        // Optional: sideways drift slide
        if (isDrifting && Mathf.Abs(turnInput) > 0.1f)
        {
            Vector3 sideways = transform.right * turnInput * 0.5f;
            rb.MovePosition(rb.position + sideways * Time.fixedDeltaTime);
        }

        // Visual lean/tilt
        if (childObject != null)
        {
            float tiltAngle = -turnInput * rotationSpeed;
            Quaternion targetTilt = Quaternion.Euler(0f, 0f, tiltAngle);
            childObject.localRotation = Quaternion.Slerp(childObject.localRotation, targetTilt, Time.deltaTime * 5f);
        }
    }

    private void HandleGroundCheck()
    {
        if (rb.velocity.y <= 0.1f && IsGrounded())
        {
            if (isAirborne)
            {
                isAirborne = false;
                airTime = 0f;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.2f);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {
            rb.AddForce(transform.forward * jumpForce / 10, ForceMode.Impulse);
        }
    }
}
