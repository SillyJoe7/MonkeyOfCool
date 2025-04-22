using UnityEngine;

public class SmoothResetRotation : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrees per second
    private bool isResetting = false;
    private float resetTimer = 0f;
    private float resetDuration = 1f;

    private Transform playerChild;
    private Quaternion initialMainRotation;
    private Quaternion initialPlayerRotation;

    void Start()
    {
        // Find the child named "Player"
        playerChild = transform.Find("Player");

        // Store initial rotations
        initialMainRotation = transform.rotation;
        if (playerChild != null)
        {
            initialPlayerRotation = playerChild.rotation;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isResetting = true;
            resetTimer = 0f;
        }

        if (isResetting)
        {
            resetTimer += Time.deltaTime;

            // Calculate easing factor
            float easingFactor = Mathf.Clamp01(resetTimer / resetDuration);

            // Apply the easing effect on rotation
            transform.rotation = Quaternion.RotateTowards(initialMainRotation, Quaternion.Euler(0f, 0f, 0f), rotationSpeed * Time.deltaTime);

            if (playerChild != null)
            {
                playerChild.rotation = Quaternion.RotateTowards(initialPlayerRotation, Quaternion.Euler(0f, 90f, 0f), rotationSpeed * Time.deltaTime);
            }

            // Stop after 1 second
            if (resetTimer >= resetDuration)
            {
                isResetting = false;
            }
        }
    }
}
