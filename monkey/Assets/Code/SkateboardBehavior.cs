using UnityEngine;

public class SkateboardBehaviour : MonoBehaviour
{
    [Tooltip("A float value determining the max speed this car can go forwards or backwards.")]
    [SerializeField] private float maxSpeed = 10f;
    [Tooltip("A float value determining how fast the car accelerates.")]
    [SerializeField] private float acceleration = 10f;
    [Tooltip("A float value determining how fast the car decelerates.")]
    [SerializeField] private float deceleration = 10f;
    [Tooltip("A float value determining how fast the car can turn to the left or right.")]
    [SerializeField] private float turnRate = 180f;
    [Tooltip("A boolean value determining whether or not the car can rotate when not moving.")]
    [SerializeField] private bool onlyTurnWhileMoving = true;
    [SerializeField] private float jumpForce = 100f;

    private float speed, turnInput, moveInput;
    public bool Grounded;

    public float turnAmount
    {
        get { return turnInput * turnRate; }
    }
    // Update is called once per frame
    void Update()
    {
        float targetSpeed; // Target speed to accelerate / decelerate towards.
        turnInput = Input.GetAxisRaw("Horizontal"); // Get Horizontal input.
        moveInput = Input.GetAxisRaw("Vertical"); // Get Vertical input.

        targetSpeed = moveInput * maxSpeed;

        // Acelerate / decelerate towards target speed.
        if (targetSpeed > speed) speed = Mathf.MoveTowards(speed, targetSpeed, acceleration * Time.deltaTime);
        else speed = Mathf.MoveTowards(speed, targetSpeed, deceleration * Time.deltaTime);

        // If onlyTurnWhileMoving and not moving nullify turnInput.
        if (onlyTurnWhileMoving && Mathf.Abs(speed) == 0.0f) turnInput = 0.0f;

        // Apply rotationg and translation.
        transform.Rotate(Vector3.up * turnAmount * Time.deltaTime, Space.World);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        

        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Grounded = false;
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            Grounded = true;
        }
    }



}
