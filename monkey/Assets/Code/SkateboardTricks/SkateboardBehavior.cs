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
    [SerializeField] private Transform childObject;
    [SerializeField] private float airTiltSpeed = 5f;
    [SerializeField] private float maxTiltAngle = 30f;

    [Header("Air Points Settings")]
    [SerializeField] private float pointRate = 10f;

    [Header("Drift Settings")]
    [SerializeField] private float driftTurnMultiplier = 2f;
    [SerializeField] private float driftFrictionMultiplier = 0.5f;

    [Header("GroundCheck Settings")]
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Points settings")]
    [SerializeField] private Points_SCR points_SCR;

    private float currentSpeed = 0f;
    private float speedVelocity = 0f;
    private float turnInput, moveInput;
    private bool isDrifting = false;
    public bool isAirborne = false;
    private float airTime = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (childObject == null)
            Debug.LogWarning("Child object not assigned in inspector!");
    }

    void Update()
    {
        turnInput = Input.GetAxis("Horizontal");
        moveInput = Input.GetAxis("Vertical");
        isDrifting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space) && !isAirborne && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isAirborne = true;
            airTime = 0f;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleGroundCheck();
        HandleTilt();

        if (isAirborne)
        {
            airTime += Time.fixedDeltaTime;
        }
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
    }

    private void HandleTilt()
    {
        if (childObject == null) return;

        float tiltX = 0f;

        if (isAirborne)
        {
            float yVelocity = rb.velocity.y;
            tiltX = Mathf.Lerp(-maxTiltAngle, maxTiltAngle, Mathf.InverseLerp(-10f, 10f, yVelocity));
        }

        Quaternion targetTilt = Quaternion.Euler(tiltX, 0f, 0f);
        childObject.localRotation = Quaternion.Slerp(childObject.localRotation, targetTilt, Time.fixedDeltaTime * airTiltSpeed);
    }

    private void HandleGroundCheck()
    {
        if (rb.velocity.y <= 0.1f && IsGrounded() && isAirborne)
        {
            isAirborne = false;
            if (points_SCR != null)
            {
                points_SCR.TrickAddPoints(Mathf.FloorToInt(airTime * pointRate));
            }
            airTime = 0f;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost") && rb.velocity.magnitude < maxSpeed * 1.5f)
        {
            rb.AddForce(transform.forward * jumpForce / 10, ForceMode.Impulse);
        }
    }
}
