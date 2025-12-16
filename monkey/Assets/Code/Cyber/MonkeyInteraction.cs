using UnityEngine;
using TMPro;   // For UI
using UnityEngine.UI;
using System.Collections;

public class MonkeyInteraction : MonoBehaviour
{
    [Header("Password Settings")]
    public int passwordLength = 6;
    public float timeLimit = 10f; // seconds to type password

    [Header("UI Elements")]
    public GameObject passwordPanel;   // UI panel with InputField + timer
    public TMP_InputField inputField;
    public TMP_Text passwordDisplay;   // Shows the password
    public TMP_Text timerText;

    private string currentPassword;
    private bool isInteracting = false;

    private Transform player; // For teleporting if fail

    void Start()
    {
        passwordPanel.SetActive(false);
        player = Camera.main.transform; // or assign your player manually
    }

    // Call this from PlayerInteraction when pressing E
    public void TryInteract()
    {
        if (isInteracting) return;

        isInteracting = true;
        StartPasswordChallenge();
    }

    void StartPasswordChallenge()
    {
        // Generate random password
        currentPassword = GeneratePassword(passwordLength);

        // Show UI
        passwordPanel.SetActive(true);
        passwordDisplay.text = currentPassword;
        inputField.text = "";
        inputField.ActivateInputField();

        // Start countdown
        StartCoroutine(PasswordTimer());
    }

    IEnumerator PasswordTimer()
    {
        float timer = timeLimit;

        while (timer > 0)
        {
            timerText.text = "Time: " + Mathf.Ceil(timer);
            timer -= Time.deltaTime;

            // Check if input is correct
            if (inputField.text == currentPassword)
            {
                Success();
                yield break;
            }

            yield return null;
        }

        Fail();
    }

    void Success()
    {
        Debug.Log("✅ Password correct! Monkey disappears.");
        passwordPanel.SetActive(false);
        gameObject.SetActive(false); // Monkey disappears
        isInteracting = false;
    }

    void Fail()
    {
        Debug.Log("❌ Password failed! Teleporting player.");
        passwordPanel.SetActive(false);
        player.position = new Vector3(0f, 1f, 0f); // teleport
        isInteracting = false;
    }

    string GeneratePassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string pw = "";
        for (int i = 0; i < length; i++)
        {
            pw += chars[Random.Range(0, chars.Length)];
        }
        return pw;
    }
}
