using UnityEngine;

public class ResetOnTouch : MonoBehaviour
{
    [SerializeField] private Points_SCR pointsScript;
    public Vector3 resetPosition = new Vector3(0f, 0.3f, 0f);
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Touched by Player — resetting player position.");

            // Reset player position
            other.transform.position = resetPosition;

            // Optional: reset velocity if the player has a Rigidbody
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Halve the player's points
            if (pointsScript != null)
            {
                int currentPoints = pointsScript.GetCurrentPoints();
                int newPoints = currentPoints / 2;
                pointsScript.SetPoints(newPoints);
                Debug.Log("Points halved to: " + newPoints);
            }
            else
            {
                Debug.LogWarning("Points_SCR not assigned to ResetOnTouch.");
            }
        }
    }
}
