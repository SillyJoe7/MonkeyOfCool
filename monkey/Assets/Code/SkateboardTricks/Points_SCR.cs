using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Points_SCR : MonoBehaviour
{
    [SerializeField] private Text Points_text;
    [SerializeField] private Text Trick_text; // Add this in the Inspector for showing trick names

    int Points = 0;
    float trickDisplayTime = 2f; // How long to show trick name
    float trickTimer = 0f;

    void Update()
    {
        // Hide trick text after time expires
        if (Trick_text != null && trickTimer > 0)
        {
            trickTimer -= Time.deltaTime;
            if (trickTimer <= 0)
            {
                Trick_text.text = "";
            }
        }
    }

    public void TrickAddPoints(string trickName, int addPoints)
    {
        Points += addPoints;

        if (Points_text != null)
        {
            Points_text.text = "Aura Points: " + Points;
        }

        if (Trick_text != null)
        {
            Trick_text.text = $"{trickName}! +{addPoints} points";
            trickTimer = trickDisplayTime;
        }
        else
        {
            Debug.Log($"Trick: {trickName} (+{addPoints} points)");
        }
    }

    public void TrickAddPoints(int addPoints) => TrickAddPoints("Trick", addPoints);
    public void TrickAddPoints() => TrickAddPoints("Trick", 10);

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetCurrentPoints() => Points;

    public void SetPoints(int newAmount)
    {
        Points = newAmount;
        if (Points_text != null)
            Points_text.text = "Aura Points: " + Points;
    }
}
