using UnityEngine;

public class DoorScore : MonoBehaviour
{
    [Header("Score Requirement")]
    public int requiredPoints = 5; // Bananas needed to open

    [Header("Door Models")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    public bool isUnlocked = false; // change from private to public


    // Called by PlayerInteraction when pressing E
    public void TryInteract()
    {
        if (isUnlocked)
            return;

        if (ScoreManager.instance == null)
        {
            Debug.LogError("❌ ScoreManager instance not found!");
            return;
        }

        if (ScoreManager.instance.score < requiredPoints)
        {
            Debug.Log($"🍌 Need {requiredPoints} bananas to open this door!");
            return;
        }

        UnlockDoor();
    }

    private void UnlockDoor()
    {
        isUnlocked = true;
        doorClosed.SetActive(false);
        doorOpen.SetActive(true);

        Debug.Log("🚪 Door opened!");
    }
}
