using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private SkateboardBehaviour skateboard;
    [SerializeField] private float yProximityThreshold = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
            Debug.LogError("PlayerJump: No Rigidbody on player.");
        else
            Debug.Log("PlayerJump: Rigidbody found on player.");

        if (skateboard == null)
            Debug.LogError("PlayerJump: SkateboardBehaviour not assigned.");
    }

    void Update()
    {
        // Space jump (grounded)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("PlayerJump: SPACE jump.");
            Jump();
        }

        // F jump (proximity to board)
        if (Input.GetKeyDown(KeyCode.F) && IsCloseToBoard())
        {
            Debug.Log("PlayerJump: F proximity jump.");
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reset Y velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        float rayDistance = 0.3f;
        Vector3 origin = transform.position;
        bool grounded = Physics.Raycast(origin, Vector3.down, rayDistance, LayerMask.GetMask("Default"));
        Debug.DrawRay(origin, Vector3.down * rayDistance, grounded ? Color.green : Color.red, 0.1f);
        return grounded;
    }

    bool IsCloseToBoard()
    {
        float yDistance = Mathf.Abs(transform.position.y - skateboard.transform.position.y);
        return yDistance <= yProximityThreshold;
    }
}
