using UnityEngine;
using TMPro;

public class InteractPrompt : MonoBehaviour
{
    public TMP_Text promptText;         // Drag your TMP_Text here
    public float detectRange = 3f;      // How close to detect a door
    public LayerMask doorLayer;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckForDoor();
    }

    void CheckForDoor()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectRange, doorLayer))
        {
            // Try score door first
            DoorScore scoreDoor = hit.collider.GetComponentInParent<DoorScore>();
            if (scoreDoor != null)
            {
                int playerScore = ScoreManager.instance.score;

                if (scoreDoor.isUnlocked) // optional if you want prompt to disappear
                {
                    promptText.gameObject.SetActive(false);
                    return;
                }

                if (playerScore < scoreDoor.requiredPoints)
                {
                    int needed = scoreDoor.requiredPoints - playerScore;
                    promptText.text = $"You need {needed} more bananas! :(";
                }
                else
                {
                    promptText.text = "Press E to interact";
                }

                promptText.gameObject.SetActive(true);
                return;
            }

            // Try question door
            DoorInteraction questionDoor = hit.collider.GetComponentInParent<DoorInteraction>();
            if (questionDoor != null)
            {
                promptText.text = "Press E to interact";
                promptText.gameObject.SetActive(true);
                return;
            }

            // If neither door, hide prompt
            promptText.gameObject.SetActive(false);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
