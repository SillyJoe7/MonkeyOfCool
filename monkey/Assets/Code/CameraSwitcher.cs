using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera;
    public Camera cutsceneCamera;

    [Header("Cutscene Settings")]
    public float cutsceneDuration = 5f;

    [Header("Particle Effects")]
    public ParticleSystem cutsceneParticles; // Drag your particle system here

    [Header("Countdown")]
    public CountdownBar countdownBar; // Reference to the countdown bar script

    void Start()
    {
        StartCoroutine(SwitchToCutscene());
    }

    IEnumerator SwitchToCutscene()
    {
        // Switch to cutscene camera
        mainCamera.enabled = false;
        cutsceneCamera.enabled = true;

        // Play particles
        if (cutsceneParticles != null)
            cutsceneParticles.Play();

        // Wait for the cutscene duration
        yield return new WaitForSeconds(cutsceneDuration);

        // Stop particles
        if (cutsceneParticles != null)
            cutsceneParticles.Stop();

        // Switch back to the main camera
        cutsceneCamera.enabled = false;
        mainCamera.enabled = true;

        // Start countdown after cutscene ends
        if (countdownBar != null)
            countdownBar.BeginCountdown();
    }
}
