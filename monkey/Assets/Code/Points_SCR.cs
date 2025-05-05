using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Points_SCR : MonoBehaviour
{
    [SerializeField] private Text Points_text;

    int Points = 0;

    // Default trick points (calls the main version with a fixed value)
    public void TrickAddPoints()
    {
        TrickAddPoints(10); // This ensures consistent point handling
    }

    // Main method for adding any number of points
    public void TrickAddPoints(int AddPoints)
    {
        Points += AddPoints;

        if (Points_text != null)
        {
            Points_text.text = "Aura Points: " + Points;  // Update the UI text to reflect points
        }
        else
        {
            Debug.LogWarning("Points_text UI reference is not set!");
        }
    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public int GetCurrentPoints()
    {
        return Points;
    }
    public void SetPoints(int newAmount)
    {
        Points = newAmount;

        if (Points_text != null)
        {
            Points_text.text = "Aura Points: " + Points;
        }
    }


}
