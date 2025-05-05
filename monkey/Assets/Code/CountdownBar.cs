using UnityEngine;
using UnityEngine.UI;

public class CountdownBar : MonoBehaviour
{
    public Slider countdownSlider;
    public float countdownTime = 5f;
    public MonoBehaviour scriptToRun;
    public string methodName = "OnCountdownEnd";

    private float timeLeft;
    private bool counting = false;
    private bool hasTriggered = false;

    void Start()
    {
        timeLeft = countdownTime;
        countdownSlider.maxValue = countdownTime;
        countdownSlider.value = countdownTime;
    }

    void Update()
    {
        if (!counting || hasTriggered) return;

        timeLeft -= Time.deltaTime;
        countdownSlider.value = timeLeft;

        if (timeLeft <= 0f)
        {
            hasTriggered = true;
            if (scriptToRun != null && !string.IsNullOrEmpty(methodName))
            {
                scriptToRun.Invoke(methodName, 0f);
            }
        }
    }

    public void BeginCountdown()
    {
        counting = true;
    }
}
