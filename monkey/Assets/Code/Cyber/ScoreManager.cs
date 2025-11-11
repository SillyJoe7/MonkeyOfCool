using UnityEngine;
using UnityEngine.UI; // only if you�re displaying score in UI

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // allows global access
    public int score = 0;
    public Text scoreText; // optional, if using UI Text

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoint()
    {
        score++;
        Debug.Log("Score: " + score);

        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
    public void AddFive()
    {
        score+= 5;
        Debug.Log("Score: " + score);

        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
