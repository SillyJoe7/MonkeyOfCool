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
    public Button[] choiceButtons;
    public TMP_Text[] choiceTexts;

    private InteractableQuiz currentQuiz;
    private int[] shuffledIndices;

    public bool QuestionPanelActive => questionPanel.activeSelf;

    void Awake()
    {
        Instance = this;
        questionPanel.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    // Called by InteractableQuiz.TryInteract()
    public void ShowQuestion(InteractableQuiz quiz)
    {
        currentQuiz = quiz;

        var q = quiz.GetCurrentQuestion();

        questionText.text = q.questionText;
        characterImage.sprite = q.characterSprite;

        // Create array for shuffling
        shuffledIndices = new int[q.choices.Length];
        for (int i = 0; i < shuffledIndices.Length; i++)
            shuffledIndices[i] = i;

        // Fisher–Yates shuffle
        for (int i = shuffledIndices.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledIndices[i], shuffledIndices[j]) = (shuffledIndices[j], shuffledIndices[i]);
        }

        // Assign buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < q.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);

                int realIndex = shuffledIndices[i];
                choiceTexts[i].text = q.choices[realIndex];

                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => SubmitAnswer(realIndex));
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

        currentQuiz.TryAnswer(choiceIndex);
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
            remaining -= Time.unscaledDeltaTime;
            yield return null;
        }
        cooldownText.gameObject.SetActive(false);
    }
}
