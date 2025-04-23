using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirbornePitchWithGroundReset : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            // Keep facing forward, but flatten rotation (no pitch/roll)
            Vector3 flatForward = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (flatForward.sqrMagnitude > 0.001f)
            {
                Quaternion flatRotation = Quaternion.LookRotation(flatForward.normalized);
                transform.rotation = flatRotation;
            }
        }
        else
        {
            Vector3 velocity = rb.velocity;

            if (velocity.sqrMagnitude > 0.01f)
            {
                Vector3 flatForward = new Vector3(velocity.x, 0, velocity.z);
                if (flatForward.sqrMagnitude < 0.001f) return;

                Quaternion baseRotation = Quaternion.LookRotation(flatForward.normalized);
                float pitch = Mathf.Atan2(velocity.y, flatForward.magnitude) * Mathf.Rad2Deg;
                Quaternion pitchRotation = Quaternion.AngleAxis(-pitch, Vector3.right);

                transform.rotation = baseRotation * pitchRotation;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
