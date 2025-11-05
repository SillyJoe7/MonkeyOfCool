using UnityEngine;

public class KillBoxAndReset : MonoBehaviour
{
    [SerializeField] private Transform ScateboardTF;
    [SerializeField] private Rigidbody ScateboardRB;
    [SerializeField] private float flipThreshold = 100f;
    [SerializeField] private float uprightSpeed = 5f; // how fast to flip upright

    private bool flipping;

    void Update()
    {
        float angle = Vector3.Angle(ScateboardTF.up, Vector3.up);

        // If upside down and not already flipping
        if (angle > flipThreshold && !flipping)
        {
            Debug.Log("Player is upside down, flipping upright...");
            StartCoroutine(FlipUpright());
        }
    }

    private System.Collections.IEnumerator FlipUpright()
    {
        flipping = true;

        // Freeze movement while flipping
        ScateboardRB.velocity = Vector3.zero;
        ScateboardRB.angularVelocity = Vector3.zero;
        ScateboardRB.isKinematic = true;

        Quaternion targetRotation = Quaternion.LookRotation(ScateboardTF.forward, Vector3.up);
        float t = 0f;

        // Smoothly rotate upright over time
        while (Quaternion.Angle(ScateboardTF.rotation, targetRotation) > 1f)
        {
            ScateboardTF.rotation = Quaternion.Slerp(ScateboardTF.rotation, targetRotation, Time.deltaTime * uprightSpeed);
            yield return null;
            t += Time.deltaTime;
        }

        // Unfreeze and re-enable physics
        ScateboardRB.isKinematic = false;

        flipping = false;
        Debug.Log("Flip complete!");
    }
}
