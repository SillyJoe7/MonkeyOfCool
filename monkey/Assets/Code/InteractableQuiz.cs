using UnityEngine;

public class InteractableQuiz : MonoBehaviour
{
    [System.Serializable]
    public struct QuestionData
    {
        public string questionText;
        public string[] choices;
        public int correctChoiceIndex;
        public Sprite characterSprite;
    }

    [Header("Quiz Settings")]
    public QuestionData[] questions;     // All questions assigned in the Inspector
    public int questionsToAsk = 1;       // Total correct answers needed to unlock

    private int currentQuestion = 0;
    private int correctCount = 0;

    [Header("Models (Locked/Unlocked)")]
    public GameObject modelLocked;
    public GameObject modelUnlocked;

    private bool isUnlocked = false;
    private bool onCooldown = false;
    private float cooldownTime = 60f;
    private float timer = 0f;

    void Update()
    {
        if (onCooldown)
        {
            timer -= Time.unscaledDeltaTime;
            if (timer <= 0)
                onCooldown = false;
        }
    }

    // Called when player hits E on this object
    public void TryInteract()
    {
        if (isUnlocked) return;
        if (onCooldown)
        {
            Debug.Log("Object is on cooldown!");
            return;
        }

        AskCurrentQuestion();
    }

    private void AskCurrentQuestion()
    {
        // If already completed required questions
        if (correctCount >= questionsToAsk)
        {
            Unlock();
            return;
        }

        // Prevent index errors
        if (currentQuestion >= questions.Length)
        {
            Debug.LogError("Not enough questions assigned!");
            return;
        }

        UIQuestionManager.Instance.ShowQuestion(this);
    }

    public QuestionData GetCurrentQuestion()
    {
        return questions[currentQuestion];
    }

    public void TryAnswer(int choiceIndex)
    {
        QuestionData q = questions[currentQuestion];

        if (choiceIndex == q.correctChoiceIndex)
        {
            // Correct answer
            correctCount++;
            currentQuestion++;

            if (correctCount >= questionsToAsk)
            {
                Unlock();
            }
            else
            {
                AskCurrentQuestion();
            }
        }
        else
        {
            // WRONG ANSWER → Reset quiz progress
            correctCount = 0;
            currentQuestion = 0;

            StartCooldown();
        }
    }

    private void Unlock()
    {
        isUnlocked = true;

        if (modelLocked) modelLocked.SetActive(false);
        if (modelUnlocked) modelUnlocked.SetActive(true);

        Debug.Log("✅ Object unlocked!");
    }

    private void StartCooldown()
    {
        onCooldown = true;
        timer = cooldownTime;

        UIQuestionManager.Instance.ShowCooldown(timer);

        Debug.Log("❌ Wrong answer — cooldown started. Quiz reset.");
    }
}
