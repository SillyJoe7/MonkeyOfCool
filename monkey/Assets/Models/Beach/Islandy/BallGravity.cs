using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 0.5f;      // Custom gravity scale
    public float bounceForce = 5f;         // How strong the push is
    public int hitPoints = 15;             // Points awarded on hit

    private Rigidbody rb;
    private Points_SCR pointsManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Use custom gravity only

        // Look for the Points_SCR component in the scene
        pointsManager = FindObjectOfType<Points_SCR>();
        if (pointsManager == null)
        {
            Debug.LogWarning("CustomGravity: No Points_SCR found in the scene.");
        }
    }

    void FixedUpdate()
    {
        Vector3 gravity = Physics.gravity * gravityScale;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Beachball hit by player!");

            // Calculate direction from player to ball, then normalize it
            Vector3 directionAway = (transform.position - collision.transform.position).normalized;

            // Apply force away from the player
            rb.AddForce(directionAway * bounceForce, ForceMode.Impulse);

            // Award points if Points_SCR exists
            if (pointsManager != null)
            {
                pointsManager.TrickAddPoints("Beachball Bounce", hitPoints);
            }
        }
    }
}
