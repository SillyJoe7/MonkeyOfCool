using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public string groundTag = "Ground";
    public float jumpForce = 5f;

    private bool isTouchingGround = false;
    private bool wasInAir = false;
    private float rotationAccumulated = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody required for jumping.");
        }
    }

    void Update()
    {
        // Jump when on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isTouchingGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isTouchingGround = false;
        }

        // Rotate on the X-axis using W and S keys when in the air
        if (Input.GetKey(KeyCode.LeftShift) && !isTouchingGround)
        {
            float rotationThisFrame = 0f;

            if (Input.GetKey(KeyCode.W)) // W key
            {
                rotationThisFrame = rotationSpeed * Time.deltaTime;  // Positive rotation on X-axis
                transform.Rotate(Vector3.right, rotationThisFrame);
            }
            else if (Input.GetKey(KeyCode.S)) // S key
            {
                rotationThisFrame = -rotationSpeed * Time.deltaTime;  // Negative rotation on X-axis
                transform.Rotate(Vector3.right, rotationThisFrame);
            }

            wasInAir = true;
            rotationAccumulated += Mathf.Abs(rotationThisFrame);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            if (wasInAir && !isTouchingGround)
            {
                Debug.Log("Rotated while in air: " + rotationAccumulated + " degrees");
                rotationAccumulated = 0f;
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
