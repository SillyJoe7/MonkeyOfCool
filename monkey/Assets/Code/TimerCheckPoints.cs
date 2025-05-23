using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerCheckPoints : MonoBehaviour
{
    public float countdownTime = 30f;
    public TextMeshProUGUI timerText; // Use Text if not using TMP
    public int requiredPointsToWin = 50;
    public Points_SCR pointsScript;

    private float timeLeft;
    private bool hasEnded = false;

    void Start()
    {
        timeLeft = countdownTime;
    }

    void Update()
    {
        if (hasEnded) return;

        timeLeft -= Time.deltaTime;

        // Display time left
        if (timerText != null)
        {
            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft).ToString() + "s";
        }

        if (timeLeft <= 0f)
        {
            hasEnded = true;
            Debug.Log("Timer has ended.");
            CheckPoints();
        }
    }

    void CheckPoints()
    {
        if (pointsScript == null)
        {
            Debug.LogError("Points_SCR reference is missing!");
            return;
        }

        int currentPoints = pointsScript.GetCurrentPoints();
        Debug.Log("Current Points: " + currentPoints);

        if (currentPoints < requiredPointsToWin)
        {
            Debug.Log("You lost! Not enough points.");
            GameManager.LoadScene("LoseScreen");
        }
        else
        {
            Debug.Log("You won! Enough points collected.");
            GameManager.LoadScene("WinScreen");
        }

    }
}
