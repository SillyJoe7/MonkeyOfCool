using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Models")]
    public GameObject doorClosed;    // Assign the closed door model
    public GameObject doorOpen;      // Assign the open door model

    [Header("Multiple Choice Question")]
    public string questionText = "What is a strong password example?";
    public string[] choices = { "12345", "password123", "qwerty", "abc123" };
    public int correctChoiceIndex = 1; // Index of the correct choice in the array
    public Sprite characterSprite;     // 2D character to show for this door

    private bool isUnlocked = false;
    private bool onCooldown = false;
    private float cooldownTime = 60f;
    private float timer = 0f;

    void Update()
    {
        if (onCooldown)
        {
            // Use unscaled delta so cooldown works even when game is paused
            timer -= Time.unscaledDeltaTime;
            if (timer <= 0)
                onCooldown = false;
        }
    }

    // Called by the PlayerInteraction script when player presses E near this door
    public void TryInteract()
    {
        if (isUnlocked) return;       // Already unlocked
        if (onCooldown)
        {
            Debug.Log("Door is on cooldown!");
            return;
        }

        // Show the question panel with choices
        UIQuestionManager.Instance.ShowQuestion(this);
    }

    // Called by UIQuestionManager when a choice button is clicked
    public void TryAnswer(int choiceIndex)
    {
        if (choiceIndex == correctChoiceIndex)
        {
            UnlockDoor();
        }
        else
        {
            StartCooldown();
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;
        doorClosed.SetActive(false);
        doorOpen.SetActive(true);
        Debug.Log("✅ Door opened!");
    }

    private void StartCooldown()
    {
        onCooldown = true;
        timer = cooldownTime;
        UIQuestionManager.Instance.ShowCooldown(timer);
        Debug.Log("❌ Wrong answer — cooldown started.");
    }
}
