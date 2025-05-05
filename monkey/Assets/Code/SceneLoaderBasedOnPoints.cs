using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderBasedOnPoints : MonoBehaviour
{
    // Reference to the Points_SCR script (drag it in the inspector)
    [SerializeField] private Points_SCR pointsScript;

    // The threshold of points needed to load SceneA
    [SerializeField] private int pointsThreshold = 50; // Change this threshold based on your need

    // The scene names (make sure they are in Build Settings)
    [SerializeField] private string sceneIfPointsAreEnough = "BeauTest"; // Scene to load if enough points
    [SerializeField] private string sceneIfPointsAreTooLow = "TitleScreen"; // Scene to load if not enough points

    // Method to check points and load scenes
    public void CheckPointsAndLoadScene()
    {
        if (pointsScript != null)
        {
            // Get current points from the Points_SCR script
            int currentPoints = pointsScript.GetCurrentPoints();  // Assuming GetCurrentPoints() returns the current points

            // Check if the points are greater than or equal to the threshold
            if (currentPoints >= pointsThreshold)
            {
                Debug.Log("Points are enough! Loading " + sceneIfPointsAreEnough);
                SceneManager.LoadScene(sceneIfPointsAreEnough);
            }
            else
            {
                Debug.Log("Not enough points! Loading " + sceneIfPointsAreTooLow);
                SceneManager.LoadScene(sceneIfPointsAreTooLow);
            }
        }
        else
        {
            Debug.LogError("Points_SCR script is not assigned in the inspector.");
        }
    }
}
