using UnityEngine;

public class KillBoxAndReset : MonoBehaviour
{
    [SerializeField] private Transform ScateboardTF;
    [SerializeField] private Rigidbody ScateboardRB;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private SkateboardBehaviour SbBh;

    [Header("Points System")]
    [SerializeField] private Points_SCR pointsScript;  // Reference to Points_SCR

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            print("Touched Ground");

            // Halve the points
            if (pointsScript != null)
            {
                int currentPoints = pointsScript.GetCurrentPoints();
                int newPoints = currentPoints / 2;

                pointsScript.SetPoints(newPoints); // New method needed in Points_SCR
                Debug.Log("Points halved to: " + newPoints);
            }
            else
            {
                Debug.LogWarning("Points_SCR not assigned to KillBoxAndReset.");
            }

            resetLocAndRot();
        }
    }

    private void resetLocAndRot()
    {
        if (SpawnPoint != null)
        {
            ScateboardTF.position = SpawnPoint.position;
            ScateboardTF.rotation = SpawnPoint.rotation;
            ScateboardRB.velocity = Vector3.zero;
        }
    }
}
