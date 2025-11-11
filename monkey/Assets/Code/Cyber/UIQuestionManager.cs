using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIQuestionManager : MonoBehaviour
{
    public static UIQuestionManager Instance;

    [Header("UI Elements")]
    public GameObject questionPanel;
    public TMP_Text questionText;
    public Image characterImage;
    public TMP_Text cooldownText;

    [Header("Choice Buttons")]
    public Button[] choiceButtons;    // 4 buttons
    public TMP_Text[] choiceTexts;    // Text of each button

    private DoorInteraction currentDoor;
    private int[] shuffledIndices;

    public bool QuestionPanelActive => questionPanel.activeSelf; // For camera & player scripts

    void Awake()
    {
        Instance = this;
        questionPanel.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    public void ShowQuestion(DoorInteraction door)
    {
        currentDoor = door;

        questionText.text = door.questionText;
        characterImage.sprite = door.characterSprite;

        // Shuffle choices
        shuffledIndices = new int[door.choices.Length];
        for (int i = 0; i < door.choices.Length; i++)
            shuffledIndices[i] = i;

        // Fisher-Yates shuffle
        for (int i = shuffledIndices.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = shuffledIndices[i];
            shuffledIndices[i] = shuffledIndices[j];
            shuffledIndices[j] = temp;
        }

        // Assign buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < door.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                int choiceIndex = shuffledIndices[i];
                choiceTexts[i].text = door.choices[choiceIndex];

                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => SubmitAnswer(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }

        questionPanel.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SubmitAnswer(int choiceIndex)
    {
        // Resume game
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDoor.TryAnswer(choiceIndex);
        questionPanel.SetActive(false);
    }

    public void ShowCooldown(float time)
    {
        cooldownText.gameObject.SetActive(true);
        StartCoroutine(CooldownTimer(time));
    }

    private IEnumerator CooldownTimer(float time)
    {
        float remaining = time;
        while (remaining > 0)
        {
            cooldownText.text = "Try again in " + Mathf.CeilToInt(remaining) + "s";
            remaining -= Time.unscaledDeltaTime; // Works while timeScale = 0
            yield return null;
        }
        cooldownText.gameObject.SetActive(false);
    }
}
